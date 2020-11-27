using NLog;
using System.IO;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using System;

namespace Messenger.PictureService
{
    internal class ImageProcessor
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();
        private const int LARGE_SIZE_WIDTH = 1920;
        private const int MEDIUM_SIZE_WIDTH = 960;
        private const int SMALL_SIZE_WIDTH = 480;

        internal static bool ResizeImages(User user)
        {
            try
            {
                TryResize(user);
                return true;
            }
            catch (Exception e)
            {
                m_logger.Error($"Got error while resizing an image {e.Message} {e.StackTrace}");
            }

            return false;
        }

        private static bool TryResize(User user)
        {
            m_logger.Trace("Starting resizing image");

            if (string.IsNullOrEmpty(user.ImageLargeLink))
            {
                m_logger.Trace("Image link is empty");
                return false;
            }

            if (!File.Exists(user.ImageLargeLink))
            {
                m_logger.Error($"File {user.ImageLargeLink} does not exists");
                return false;
            }

            string tempName = user.ImageLargeLink;
            string original = AddSuffixForFile(user.ImageLargeLink, "_original");
            m_logger.Trace($"Dumping temp original file: {user.ImageLargeLink} -> {original}");
            File.Copy(user.ImageLargeLink, original);

            user.ImageLargeLink = ResizeTo(original, LARGE_SIZE_WIDTH, AddSuffixForFile(tempName, "_large"));
            user.ImageMediumLink = ResizeTo(original, MEDIUM_SIZE_WIDTH, AddSuffixForFile(tempName, "_medium"));
            user.ImageSmallLink = ResizeTo(original, SMALL_SIZE_WIDTH, AddSuffixForFile(tempName, "_small"));

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="originalFile">Original file - file for reading</param>
        /// <param name="targetWidth">Target width</param>
        /// <param name="targetFilename">Target filename</param>
        /// <returns></returns>
        private static string ResizeTo(string originalFile, int targetWidth, string targetFilename)
        {
            m_logger.Info($"Resizing image to {targetWidth}");

            Image image = Image.Load(originalFile);
            image.Mutate(x => x.Resize(targetWidth, 0));

            m_logger.Info($"Saving image as {targetFilename}");
            image.SaveAsPng(targetFilename);

            return targetFilename;
        }

        private static string AddSuffixForFile(string filepath, string prefix)
        {
            return $"{Path.GetFileNameWithoutExtension(filepath)}{prefix}{Path.GetExtension(filepath)}";
        }
    }
}
