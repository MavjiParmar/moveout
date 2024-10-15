namespace BoxnMoveAPI.Helpers
{
    public static class FileHelper
    {
        public static bool IsFileNameValid(string fileName, int maxLength)
        {
            return fileName.Length <= maxLength;
        }

        public static bool IsFileSizeValid(long fileSize, long maxSizeInBytes)
        {
            return fileSize <= maxSizeInBytes;
        }
    }
}
