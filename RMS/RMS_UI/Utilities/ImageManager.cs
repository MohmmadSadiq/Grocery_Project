using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace RMS_UI.Utilities
{
    /// <summary>
    /// Manages product images: save, load, delete, and resize operations.
    /// All product images are stored in the ProductImages folder within the application data directory.
    /// </summary>
    public static class ImageManager
    {
        #region Constants
        private const string ImageFolderName = "ProductImages";
        private const int MaxImageSizeBytes = 5 * 1024 * 1024; // 5MB
        private const int ThumbnailSize = 50; // For grid view
        private const int PreviewSize = 200; // For dialog preview
        private const int MaxImageDimension = 800; // Max width/height for saved images
        #endregion

        #region Properties
        /// <summary>
        /// Gets the full path to the product images folder.
        /// </summary>
        public static string ImageFolder
        {
            get
            {
                string appData = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "RMS",
                    ImageFolderName
                );
                
                if (!Directory.Exists(appData))
                    Directory.CreateDirectory(appData);
                
                return appData;
            }
        }
        #endregion

        #region Save Operations
        /// <summary>
        /// Saves an image from a file path, generating a unique filename.
        /// </summary>
        /// <param name="filePath">The source image file path</param>
        /// <returns>The relative path to the saved image, or null if failed</returns>
        public static string? SaveProductImage(string filePath)
        {
            try
            {
                // Validate file size
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Length > MaxImageSizeBytes)
                {
                    throw new InvalidOperationException($"Image file is too large. Maximum size is {MaxImageSizeBytes / 1024 / 1024}MB.");
                }

                // Load image into memory to avoid file locking
                byte[] imageBytes = File.ReadAllBytes(filePath);
                using (var ms = new MemoryStream(imageBytes))
                using (var image = Image.FromStream(ms))
                {
                    // Resize if needed
                    using (Image resizedImage = ResizeImage(image, MaxImageDimension, MaxImageDimension))
                    {
                        // Generate unique filename using timestamp and GUID
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
                        string fileName = $"product_{timestamp}_{uniqueId}.jpg";
                        string fullPath = Path.Combine(ImageFolder, fileName);

                        // Save as JPEG with good quality
                        var encoder = GetEncoder(ImageFormat.Jpeg);
                        using (var encoderParams = new EncoderParameters(1))
                        {
                            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 85L);
                            resizedImage.Save(fullPath, encoder, encoderParams);
                        }

                        return fileName; // Return just the filename, not full path
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Saves an image for a product, replacing any existing image.
        /// </summary>
        /// <param name="image">The image to save</param>
        /// <param name="productId">The product ID</param>
        /// <param name="existingImagePath">Path to existing image to delete (optional)</param>
        /// <returns>The relative path to the saved image, or null if failed</returns>
        public static string? SaveProductImage(Image image, int productId, string? existingImagePath = null)
        {
            try
            {
                // Delete existing image if provided
                if (!string.IsNullOrEmpty(existingImagePath))
                {
                    DeleteImage(existingImagePath);
                }

                // Create a copy of the image to avoid issues with locked resources
                using (var imageCopy = new Bitmap(image))
                using (Image resizedImage = ResizeImage(imageCopy, MaxImageDimension, MaxImageDimension))
                {
                    // Generate unique filename
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fileName = $"product_{productId}_{timestamp}.jpg";
                    string fullPath = Path.Combine(ImageFolder, fileName);

                    // Save as JPEG with good quality
                    var encoder = GetEncoder(ImageFormat.Jpeg);
                    using (var encoderParams = new EncoderParameters(1))
                    {
                        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 85L);
                        resizedImage.Save(fullPath, encoder, encoderParams);
                    }

                    return fileName; // Return just the filename, not full path
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Saves an image from a file path.
        /// </summary>
        public static string? SaveProductImageFromFile(string filePath, int productId, string? existingImagePath = null)
        {
            try
            {
                // Validate file size
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Length > MaxImageSizeBytes)
                {
                    throw new InvalidOperationException($"Image file is too large. Maximum size is {MaxImageSizeBytes / 1024 / 1024}MB.");
                }

                // Load image into memory to avoid file locking
                byte[] imageBytes = File.ReadAllBytes(filePath);
                using (var ms = new MemoryStream(imageBytes))
                using (var image = Image.FromStream(ms))
                {
                    return SaveProductImage(image, productId, existingImagePath);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Load Operations
        /// <summary>
        /// Loads a product image from its path.
        /// </summary>
        /// <param name="imagePath">The image filename or relative path</param>
        /// <returns>The loaded image, or null if not found</returns>
        public static Image? LoadImage(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return null;

            try
            {
                string fullPath = GetFullPath(imagePath);
                
                if (!File.Exists(fullPath))
                    return null;

                // Load image into memory to avoid file locking
                byte[] imageBytes = File.ReadAllBytes(fullPath);
                using (var ms = new MemoryStream(imageBytes))
                {
                    // Create a copy of the image so the stream can be disposed
                    using (var originalImage = Image.FromStream(ms))
                    {
                        // Return a deep copy that doesn't depend on the stream
                        return new Bitmap(originalImage);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Loads a thumbnail version of the image for grid display.
        /// Returns placeholder if image not found.
        /// </summary>
        public static Image LoadThumbnail(string? imagePath)
        {
            var image = LoadImage(imagePath);
            if (image == null)
                return GetPlaceholderImage(ThumbnailSize);

            try
            {
                var thumbnail = ResizeImage(image, ThumbnailSize, ThumbnailSize);
                return thumbnail;
            }
            finally
            {
                // Always dispose the original image since ResizeImage creates a new one
                image.Dispose();
            }
        }

        /// <summary>
        /// Loads a preview version of the image for dialog display.
        /// Returns placeholder if image not found.
        /// </summary>
        public static Image LoadPreview(string? imagePath)
        {
            var image = LoadImage(imagePath);
            if (image == null)
                return GetPlaceholderImage(PreviewSize);

            try
            {
                var preview = ResizeImage(image, PreviewSize, PreviewSize);
                return preview;
            }
            finally
            {
                // Always dispose the original image since ResizeImage creates a new one
                image.Dispose();
            }
        }
        #endregion

        #region Delete Operations
        /// <summary>
        /// Deletes a product image.
        /// </summary>
        public static bool DeleteImage(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return true;

            try
            {
                string fullPath = GetFullPath(imagePath);
                
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Placeholder
        /// <summary>
        /// Gets a NEW placeholder image for products without images.
        /// Always creates a new image to prevent issues when the caller disposes it.
        /// </summary>
        public static Image GetPlaceholderImage(int size = PreviewSize)
        {
            // Always create a new placeholder to prevent disposal issues
            int targetSize = size <= ThumbnailSize ? ThumbnailSize : PreviewSize;
            return CreatePlaceholder(targetSize);
        }

        private static Image CreatePlaceholder(int size)
        {
            var bitmap = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.FromArgb(243, 244, 246)); // Light gray background
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Draw camera icon or "No Image" text
                using (var font = new Font("Segoe UI", size / 4f))
                using (var brush = new SolidBrush(Color.FromArgb(156, 163, 175)))
                {
                    string text = "📷";
                    var textSize = g.MeasureString(text, font);
                    float x = (size - textSize.Width) / 2;
                    float y = (size - textSize.Height) / 2;
                    g.DrawString(text, font, brush, x, y);
                }
            }
            return bitmap;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Gets the full path for an image filename.
        /// </summary>
        public static string GetFullPath(string imagePath)
        {
            if (Path.IsPathRooted(imagePath))
                return imagePath;
            
            return Path.Combine(ImageFolder, imagePath);
        }

        /// <summary>
        /// Resizes an image while maintaining aspect ratio.
        /// Always returns a NEW image copy (never returns the original).
        /// </summary>
        public static Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            double ratioX = (double)maxWidth / image.Width;
            double ratioY = (double)maxHeight / image.Height;
            double ratio = Math.Min(ratioX, ratioY);
            
            // If image is smaller than max, use original dimensions
            if (ratio >= 1.0)
            {
                ratio = 1.0;
            }

            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);

            // Ensure minimum size of 1x1
            if (newWidth < 1) newWidth = 1;
            if (newHeight < 1) newHeight = 1;

            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }
            return codecs[0]; // Default to first codec
        }

        /// <summary>
        /// Validates if a file is a valid image.
        /// </summary>
        public static bool IsValidImageFile(string filePath)
        {
            return IsValidImageFile(filePath, out _);
        }

        /// <summary>
        /// Validates if a file is a valid image with error message output.
        /// </summary>
        public static bool IsValidImageFile(string filePath, out string errorMessage)
        {
            errorMessage = "";
            
            try
            {
                string extension = Path.GetExtension(filePath).ToLower();
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".bmp")
                {
                    errorMessage = "Invalid file type. Please select a JPG, PNG, or BMP image.";
                    return false;
                }

                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Length > MaxImageSizeBytes)
                {
                    errorMessage = $"Image file is too large. Maximum size is {MaxImageSizeBytes / 1024 / 1024}MB.";
                    return false;
                }

                // Try to load the image to verify it's valid - use memory stream to avoid file locking
                byte[] imageBytes = File.ReadAllBytes(filePath);
                using (var ms = new MemoryStream(imageBytes))
                using (var image = Image.FromStream(ms))
                {
                    if (image.Width <= 0 || image.Height <= 0)
                    {
                        errorMessage = "Invalid image dimensions.";
                        return false;
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Could not validate image: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Gets the OpenFileDialog filter for image files.
        /// </summary>
        public static string GetImageFileFilter()
        {
            return "Image Files|*.jpg;*.jpeg;*.png;*.bmp|JPEG|*.jpg;*.jpeg|PNG|*.png|Bitmap|*.bmp";
        }
        #endregion
    }
}
