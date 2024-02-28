namespace GlowingTemplate.Helpers
{
    public static class FileExseption
    {
        public static bool CheckType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }
        public static bool CheckLength(this IFormFile file, int length)
        {
            return file.Length > 1024 * length;
        }

        public static string CreateFile(this IFormFile file, string envPath, string folder)
        {
            string filename = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(envPath, folder, filename);
            using (FileStream filestream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(filestream);
            }

            return filename;
        }

        public static void DeleteFile(this string Image, string envPath, string folder)
        {
            string path = Path.Combine(envPath, folder, Image);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

        }


    }
}
