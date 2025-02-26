using Application.Interfaces;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMeterReadingService _meterReadingService;
        public MeterReadingController(IAccountService accountService, IMeterReadingService meterReadingService)
        {
            _accountService = accountService;
            _meterReadingService = meterReadingService;
        }

        /// <summary>
        /// Process meter reading uploads. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("meter-reading-uploads")]
        public async Task<IActionResult> UploadMeterReadings([FromForm] UploadMeterReadingsRequest request)
        {
            if (!request.ValidFileStream()) { return BadRequest("No file uploaded."); }

            UploadMeterReadingsResponse response = new();
            try
            {
                List<string> meterReadings = await GetFileContentLines(request);
                List<MeterReading> validMeterReadings = GetValidMeterReadings(meterReadings);
                validMeterReadings = RemoveDuplicateAccountReads(validMeterReadings);
                int succesfulReadingsProcessed = await ProcessValidMeterReadings(validMeterReadings);

                response.Successful = succesfulReadingsProcessed;
                response.Failed = meterReadings.Count - succesfulReadingsProcessed;
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error processing file", error = ex.Message });
            }
        }

        // TODO I think the functions here could perhaps be placed somewhere better within the solution
        // but I think this is fine given time constraints
        #region Functions

        /// <summary>
        /// Return entries from the submitted file that appear to be entry related. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<List<string>> GetFileContentLines(UploadMeterReadingsRequest request)
        {
            List<string> response = new();

            using (var stream = request.FileStream.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    bool lineStartsWithAccount = line.ToLower().StartsWith("accountid");

                    if (!string.IsNullOrWhiteSpace(line) && !lineStartsWithAccount)
                    {
                        response.Add(line);
                    }
                }
            }

            return response;
        }

        /// <summary>
        /// Return a collection of meter readings for submitted entries that are in the correct format.
        /// </summary>
        /// <param name="meterReadings"></param>
        /// <returns></returns>
        private List<MeterReading> GetValidMeterReadings(List<string> meterReadings)
        {
            List<MeterReading> response = new();

            foreach (var item in meterReadings)
            {
                MeterReading meterReading;
                bool validMeterReadingEntry = ValidMeterReadingEntry(item, out meterReading);
                if (validMeterReadingEntry)
                {
                    response.Add(meterReading);
                }
            }

            return response;
        }

        /// <summary>
        /// Process valid meter readings with the database. 
        /// </summary>
        /// <param name="validMeterReadings"></param>
        /// <returns></returns>
        private async Task<int> ProcessValidMeterReadings(List<MeterReading> validMeterReadings)
        {
            int response = 0;
            foreach (var meterReading in validMeterReadings)
            {
                Account? account = await _accountService.GetAccount(meterReading.AccountId);
                if (account == null) { continue; }

                List<MeterReading> accountMeterReadings = await _meterReadingService
                    .GetMeterReadingsByAccountId(meterReading.AccountId);

                if (accountMeterReadings.Any())
                {
                    bool meterReadingAlreadyStored = accountMeterReadings
                        .Any(a => a.MeterReadValue == meterReading.MeterReadValue && a.MeterReadingDateTime == meterReading.MeterReadingDateTime);

                    if (!meterReadingAlreadyStored)
                    {
                        bool meterReadingIsNewerThanExistingAccountReads =
                            !accountMeterReadings.Any(a => a.MeterReadingDateTime > meterReading.MeterReadingDateTime);

                        if (meterReadingIsNewerThanExistingAccountReads)
                        {
                            await _meterReadingService.AddMeterReading(meterReading);
                            response++;
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    await _meterReadingService.AddMeterReading(meterReading);
                    response++;
                    continue;
                }
            }

            await _meterReadingService.SaveChangesAsync();

            return response;
        }

        /// <summary>
        /// Remove duplicate readings from submitted entries
        /// </summary>
        /// <param name="validMeterReadings"></param>
        /// <returns></returns>
        private List<MeterReading> RemoveDuplicateAccountReads(List<MeterReading> validMeterReadings)
        {
            List<MeterReading> response = new();

            foreach (var item in validMeterReadings.GroupBy(v => v.AccountId))
            {
                MeterReading mostRecentAccountReading = item
                    .OrderByDescending(a => a.MeterReadingDateTime)
                    .First();

                response.Add(mostRecentAccountReading);
            }

            return response;
        }

        /// <summary>
        /// Determine if a string input returns a valid MeterReading
        /// </summary>
        /// <param name="input"></param>
        /// <param name="meterReading"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool ValidMeterReadingEntry(string input, out MeterReading meterReading)
        {
            meterReading = new();

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var parts = input.Split(',');

            if (parts.Length < 3 || parts.Length > 4)
            {
                return false;
            }

            if (!int.TryParse(parts[0], out int accountId))
            {
                return false;
            }

            if (!DateTime.TryParse(parts[1], out DateTime meterReadingDateTime))
            {
                return false;
            }

            if (!uint.TryParse(parts[2], out uint meterReadValue) || parts[2].Length != 5)
            {
                return false;
            }

            meterReading = new MeterReading
            {
                AccountId = accountId,
                MeterReadingDateTime = meterReadingDateTime,
                MeterReadValue = (int)meterReadValue
            };

            return true;
        }
        #endregion
    }
}
