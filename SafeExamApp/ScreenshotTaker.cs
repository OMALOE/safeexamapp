﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SafeExamApp {
    class ScreenshotTaker {

        public byte[] TakeScreenshot() {
            var fileName = Guid.NewGuid().ToString(); // Path.Combine(Path.GetTempPath(), );
            try {
                TakeScreenshotWin(fileName);
            }
            catch {
                try {
                    TakeScreenshotOSX(fileName);
                }
                catch { }
            }
            try {
                return File.ReadAllBytes(fileName);
            }
            catch {
                return null;
            }
        }

        private void TakeScreenshotWin(string fileName) {            
            var captureBmp = new Bitmap(1980, 1024, PixelFormat.Format32bppArgb);
            
            using(var fs = new FileStream(fileName, FileMode.Create)) {
                using(var captureGraphic = Graphics.FromImage(captureBmp)) {
                    captureGraphic.CopyFromScreen(0, 0, 0, 0, captureBmp.Size);
                    captureBmp.Save(fs, ImageFormat.Png);
                }
            }
        }

        private void TakeScreenshotOSX(string fileName) {            
            ExecuteCaptureProcess("screencapture", $"-m -T0 -tpng -S -x {fileName}");
        }


        /// <summary>
        ///     Start execute process with parameters
        /// </summary>
        /// <param name="execModule">Application name</param>
        /// <param name="parameters">Command line parameters</param>
        /// <returns>Bytes for destination image</returns>
        private void ExecuteCaptureProcess(string execModule, string parameters) {            

            var process = Process.Start(execModule, parameters);
            if(process == null) {
                throw new InvalidOperationException(string.Format("Executable of '{0}' was not found", execModule));
            }
            process.WaitForExit();
        }
        
    }
}
