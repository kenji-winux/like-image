from minio import Minio
from minio.error import S3Error

def delete_file_from_bucket(bucket_name, file_name, access_key, secret_key):
    # Initialize minioClient with an endpoint and access/secret keys.
    minioClient = Minio('your-minio-endpoint:9000',
                        access_key=access_key,
                        secret_key=secret_key,
                        secure=False)

    # Remove the file from the bucket
    try:
        minioClient.remove_object(bucket_name, file_name)
        print(f"File {file_name} has been deleted successfully.")
    except S3Error as err:
        print(f"File deletion failed with error: {err}")

# Replace with your actual details and call the function
delete_file_from_bucket('your-bucket-name', 'your-file-name', 'your-access-key', 'your-secret-key')
