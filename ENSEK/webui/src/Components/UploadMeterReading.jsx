import { useState } from 'react';
import Dropzone from 'react-dropzone'

const UploadMeterReading = () => {
    const [total, setTotal] = useState('');
    const [successful, setSuccessful] = useState('');
    const [failed, setFailed] = useState('');
    const [showResponse, setShowResponse] = useState('');

    const handleUpload = (acceptedFiles) => {
        const url = 'https://localhost:7198/api/MeterReading/meter-reading-uploads';
        const formData = new FormData();
        formData.append('FileStream', acceptedFiles[0]);

        fetch(url, {
            method: 'POST',
            body: formData
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error. Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            setTotal(`Total entries: ${data.total}`);
            setSuccessful(`Successful: ${data.successful}`);
            setFailed(`Failed: ${data.failed}`);
            setShowResponse(true);
        })
        .catch(error => {
            console.error("Fetch error:", error);
        });
    }

    return (
        <div>
            <h2>ENSEK Upload meter readings</h2>
            <h3>Please upload meter readings in a csv format</h3>
            
            <Dropzone onDrop={handleUpload} accept={{ 'text/csv': ['.csv'] }}>
                {({ getRootProps, getInputProps }) => (
                    <section>
                        <div className="react-dropzone-container" {...getRootProps()}>
                            <input {...getInputProps()} />
                            <p>Drag n drop your file here, or click to select a file</p>
                        </div>
                    </section>
                )}
            </Dropzone>
            {
                showResponse ?
                <h4>
                    {total}<br />
                    {successful}<br />
                    {failed}<br />
                </h4>
                :
                ''
            }
        </div>
    )
}

export default UploadMeterReading;