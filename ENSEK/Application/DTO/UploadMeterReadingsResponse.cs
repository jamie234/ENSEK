namespace Domain.Models;

public class UploadMeterReadingsResponse
{
    public int Successful { get; set; }
    public int Failed { get; set; }

    public int Total
    { 
        get
        {
            return Successful + Failed;
        } 
    }
}
