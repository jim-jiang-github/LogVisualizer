using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogVisualizer.Commons
{
    public class FileOperationsHelper
    {
        public static bool IsValidFileName(string name)
        {
            string validPathPattern = @"^[^<>:""/\\|?*\x00-\x1F\x7F]+(\.[^<>:""/\\|?*\x00-\x1F\x7F]+)*$";
            return Regex.IsMatch(name, validPathPattern);
        }

        public static bool SafeResetDirectory(string directory)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                else
                {
                    Directory.Delete(directory, true);
                    Directory.CreateDirectory(directory);
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                Log.Warning("Directory {directory} delete fail: {uae}", directory, uae);
                try
                {
                    var files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                    }
                    Directory.Delete(directory, true);
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    Log.Fatal("Directory {directory} delete fail: {uae}", directory, ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Fatal("Directory {directory} delete fail: {ex}", directory, ex);
                return false;
            }
            return true;
        }

        public static bool SafeDeleteDirectory(string directory)
        {
            try
            {
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                Log.Warning("Directory {directory} delete fail: {uae}", directory, uae);
                try
                {
                    var files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                    }
                    Directory.Delete(directory, true);
                }
                catch (Exception ex)
                {
                    Log.Error("Directory {directory} delete fail: {uae}", directory, ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Directory {directory} delete fail: {ex}", directory, ex);
                return false;
            }
            return true;
        }

        public static void SafeCreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (IOException ioEx)
            {
                Log.Error("Directory not found, or path is a file:{ex}", ioEx.Message);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                Log.Error("Permission error:{ex}", uaEx.Message);
            }
        }

        public static void SafeDeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (IOException ioEx)
            {
                Log.Error("file is in use:{ex}", ioEx.Message);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                Log.Error("Permission error:{ex}", uaEx.Message);
            }
        }

        public static void SafeCreateFile(string path, string content)
        {
            try
            {
                File.WriteAllText(path, content);
            }
            catch (IOException ioEx)
            {
                Log.Error("file is in use or disk is full:{ex}", ioEx.Message);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                Log.Error("Permission error:{ex}", uaEx.Message);
            }
        }
    }
}
