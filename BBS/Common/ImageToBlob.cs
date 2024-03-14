namespace BBS.Common
{
    public class ImageToBlob
    {
        // Convert base64 string of a image to byte array
        public byte[] ConvertToBlob(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            return imageBytes;
        }
    }
}
