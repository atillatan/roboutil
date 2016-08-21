using System;
using System.Collections;
using System.IO;
using System.Text;

namespace RoboUtil.utils
{
    public class FileUtil
    {
        /// <summary>
        /// Create file or folder
        /// </summary>
        /// <param name="objectId">file or folder path, folder path must have on endof /, </param>
        /// <param name="param">for file ;value: file text content, for folder; must be null</param>
        public static void Create(string path, string fileText)
        {
            if (!FileUtil.isFolderPath(path))
            {
                string text = fileText;
                try
                {
                    path = path.Replace("/", "\\");
                    if (path.EndsWith("\\"))
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }

                    else
                    {
                        FileInfo fi = new FileInfo(path);
                        string directory = fi.DirectoryName;
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        StreamWriter sw = new StreamWriter(path);
                        sw.Write(text);
                        sw.Flush();
                        sw.Close();
                    }
                }
                catch (Exception e1)
                {
                    throw new Exception("Error occured when file saving. File: " + path, e1);
                }
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// <para>Read full file text</para>
        /// <para></para>
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns></returns>
        public static string Read(string path)
        {
            StringBuilder sb = null;
            StreamReader SR;
            string S;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
            else
            {
                sb = new StringBuilder();
                SR = File.OpenText(path);
                S = SR.ReadLine();
                while (S != null)
                {
                    sb.Append(S + "\r\n");
                    S = SR.ReadLine();
                }

                SR.Close();
                SR.Dispose();
            }
            return (sb != null ? sb.ToString() : null);
        }

        /// <summary>
        /// Update File Content
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileText"></param>
        /// <param name="append"></param>
        public static void Update(string path, string fileText, bool append)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            try
            {
                StreamWriter sw = new StreamWriter(path, append);
                sw.Write(fileText);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e2)
            {
                System.Diagnostics.Debug.WriteLine(e2.Message + "\r\n" + e2.StackTrace);
                throw new Exception("Error occured when file updating. File: " + path);
            }
        }

        /// <summary>
        /// <para>Delete file or folder</para>
        /// </summary>
        /// <param name="objectId">Path</param>
        /// <returns></returns>
        public static bool Delete(string path)
        {
            bool isDeleted = false;
            try
            {
                string tmpid = path;
                if (isFolder(tmpid))
                {
                    try
                    {
                        Directory.Delete(tmpid, true);
                    }
                    catch (Exception ex2)
                    {
                        try
                        {
                            FileInfo fi = new FileInfo(tmpid);
                            fi.Delete();
                        }
                        catch (Exception exx)
                        {
                            System.Diagnostics.Debug.WriteLine(exx.Message + exx.StackTrace);
                            throw;
                        }
                        System.Diagnostics.Debug.WriteLine(ex2.Message + ex2.StackTrace);
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception ex3)
                    {
                        System.Diagnostics.Debug.WriteLine(ex3.Message + ex3.StackTrace);
                        throw;
                    }
                }
                isDeleted = true;
            }
            catch (Exception fde)
            {
                System.Diagnostics.Debug.WriteLine(fde);
            }
            return isDeleted;
        }

        /// <summary>
        /// <para>List all file or folder</para>
        /// </summary>
        /// <param name="enableFiles">enable get File list</param>
        /// <param name="enableFolders">enable get Folder list</param>
        /// <param name="enableSubFolders">enable get SubFolder list</param>
        /// <returns>return file path list</returns>
        public static ArrayList List(string path, Boolean enableFiles, Boolean enableFolders, Boolean enableSubFolders)
        {
            ArrayList result = null;
            if (isFolderPath(path))
            {
                if (Directory.Exists(path))
                {
                    string[] arr = null;
                    if (path != null)
                    {
                        if (enableFiles || enableFolders || enableSubFolders)
                        {
                            if (enableFiles)
                            {
                                if (enableSubFolders)
                                {
                                    arr = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                                }
                                else
                                {
                                    arr = Directory.GetFiles(path);
                                }
                            }
                            else if (enableFolders)
                            {
                                if (enableSubFolders)
                                {
                                    arr = Directory.GetDirectories(path, "*.*", SearchOption.AllDirectories);
                                }
                                else
                                {
                                    arr = Directory.GetDirectories(path);
                                }
                            }
                        }
                    }
                    if (arr == null)
                    {
                        arr = Directory.GetFileSystemEntries(path);
                    }
                    ArrayList ls = new ArrayList();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        ls.Add(arr[i]);
                    }
                    result = ls;
                }
                else
                {
                    throw new DirectoryNotFoundException(path);
                }
            }
            return result;
        }

        /// <summary>
        /// List file,folder,subfolder and calculate counts
        /// </summary>
        /// <param name="path"></param>
        /// <param name="enableFiles"></param>
        /// <param name="enableFolders"></param>
        /// <param name="enableSubFolders"></param>
        /// <returns></returns>
        public static int Count(string path, Boolean enableFiles, Boolean enableFolders, Boolean enableSubFolders)
        {
            int result = 0;
            ArrayList ls = List(path, enableFiles, enableFolders, enableSubFolders);
            if (ls != null)
            {
                result = ls.Count;
            }
            return result;
        }

        /// <summary>
        /// Copy all files, folders and subfolders to dest directory
        /// </summary>
        /// <param name="source">source folder</param>
        /// <param name="dest">dest folder</param>
        /// <param name="isReplace">Is replace override</param>
        public void Copy(string source, string dest, bool isReplace)
        {
            if (isFolder(source))
            {
                DirectoryInfo sourced = new DirectoryInfo(source);
                DirectoryInfo destd = new DirectoryInfo(dest);

                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }
                CopyFilesRecursively(sourced, destd, isReplace);
            }
            else
            {
                try
                {
                    File.Copy(source, dest, isReplace);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message + ex.StackTrace);
                    throw;
                }
            }
        }

        private void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, bool isOverride)
        {
            // copy files
            foreach (FileInfo file in source.GetFiles())
            {
                try
                {
                    file.CopyTo(Path.Combine(target.FullName, file.Name), isOverride);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message + ex.StackTrace);
                    throw;
                }
            }
            // copy directories
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name), isOverride);
            }
        }

        /// <summary>
        /// Move file or folder
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public void Move(string source, string dest)
        {
            if (isFolder(source))
            {
                DirectoryInfo sourced = new DirectoryInfo(source);
                DirectoryInfo destd = new DirectoryInfo(dest);
                try
                {
                    Directory.Move(source, dest);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message + ex.StackTrace);
                    throw;
                }
            }
            else
            {
                try
                {
                    File.Move(source, dest);
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine(ex2.Message + ex2.StackTrace);
                    throw;
                }
            }
        }

        /// <summary>
        /// <para>Append file content.</para>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileText"></param>
        public static void Append(string path, string fileText)
        {
            FileUtil.Update(path, fileText, true);
        }

        /// <summary>
        /// <para>Read all content, from seperatped text file.</para>
        /// <para>User method for little file</para>
        /// <para>If your file is bigger, you must to use filestream, no buffer, no memery overload problem</para>
        /// </summary>
        /// <param name="keyIndex"></param>
        /// <param name="sep"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Hashtable ReadSeperated(int columnCount, int keyIndex, char sep, string path)
        {
            Hashtable result = new Hashtable();

            StreamReader SR;
            string line;

            if (File.Exists(path))
            {
                SR = File.OpenText(path);
                line = SR.ReadLine();
                int i = 0;
                while (line != null)
                {
                    try
                    {
                        if (line != null && line.Contains(sep.ToString()))
                        {
                            if (line.Split(sep).Length == columnCount)
                            {
                                string key = line.Split(sep)[keyIndex].ToString().Trim();
                                result.Add(key, line.Trim().Substring(line.Split(sep)[0].Length + 1).ToString());
                                i++;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error occured when Line parsing Error: \"" + line + "\", " + ex.Message + ex.StackTrace);
                        throw new Exception("Error occured when Line parsing Error: \"" + line + "\", ", ex);
                    }

                    line = SR.ReadLine();
                }
                SR.Close();
            }
            return result;
        }

        /// <summary>
        /// <para>Write text to file</para>
        /// <para></para>
        /// </summary>
        /// <param name="path">text file path</param>
        /// <param name="text">file text</param>
        public static void WriteText(string path, string text)
        {
            if (text == null)
            {
                text = "";
            }

            TextWriter tw = new StreamWriter(path);
            tw.WriteLine(text);
            tw.Close();
        }

        /// <summary>
        /// <para>Check path is file path</para>
        /// <para></para>
        /// </summary>
        /// <param name="path">file or folder path</param>
        /// <returns></returns>
        public static bool isFilePath(string path)
        {
            bool result = false;
            path = path.Replace("/", "\\");

            if (isFolder(path))
            {
                return false;
            }

            if (File.Exists(path))
            {
                // path is a file.
                return true;
            }
            else if (Directory.Exists(path))
            {
                // path is a directory.
                return false;
            }
            else
            {
                // path doesn't exist.
                path = path.Substring(path.LastIndexOf("\\"));

                if (Path.GetExtension(path) != null && Path.GetExtension(path).Length > 1)
                {
                    return true;
                }

                if (path.Contains("."))
                {
                    string extension = path.Substring(path.LastIndexOf("."));
                    if (extension.Length <= 5)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// <para>Check path is folder path</para>
        /// <para></para>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool isFolderPath(string path)
        {
            return !isFilePath(path);
        }

        /// <summary>
        /// <para>Chack existed path is folder path</para>
        /// <para>path must be exist</para>
        /// </summary>
        /// <param name="path">folder path</param>
        /// <returns></returns>
        public static bool isFolder(string path)
        {
            bool result = false;
            if (File.Exists(path))
            {
                FileAttributes attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            if (Directory.Exists(path))
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// <para>Check existed path is file path</para>
        /// <para>path must be exist</para>
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns></returns>
        public static bool isFile(string path)
        {
            return !isFolder(path);
        }

        /// <summary>
        /// <para>Check file or directory exist</para>s
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns></returns>
        public static bool isExist(string path)
        {
            bool result = false;
            if (File.Exists(path))
            {
                // path is a file.
                result = true;
            }
            else if (Directory.Exists(path))
            {
                // path is a directory.
                result = true;
            }
            else
            {
                // path doesn't exist.
                result = false;
            }
            return result;
        }

 
        public static string TryFixFilePath(string path)
        {
            string currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            return FileUtil.isExist(path) ? path
                : FileUtil.isExist(currentDir + path) ? currentDir + path
                    : FileUtil.isExist(currentDir + "\\" + path) ? currentDir + "\\" + path
                        : FileUtil.isExist(currentDir + "/" + path) ? currentDir + "/" + path
                            : FileUtil.isExist("..\\" + path) ? "..\\" + path
                                : FileUtil.isExist("../" + path) ? "../" + path : path;
        }

        public string getName(string path)
        {
            if (isFolder(path))
            {
                return getFolderName(path);
            }
            else
            {
                return getFileName(path);
            }
        }

        private string getFileName(string path)
        {
            //if (isFile(path))
            //{
            //    return path.Substring(path.LastIndexOf("\\") + 1);
            //}
            //else
            //{
            //    return null;
            //}
            FileInfo fi = new FileInfo(path);
            return fi.Name;
        }

        private string getFolderName(string path)
        {
            FileInfo fi = new FileInfo(path);
            return fi.DirectoryName;
        }

        public static string ToFileSize(long size)
        {
            if (size < 1024) { return (size).ToString("F0") + " bytes"; }
            if (size < Math.Pow(1024, 2)) { return (size / 1024).ToString("F0") + "KB"; }
            if (size < Math.Pow(1024, 3)) { return (size / Math.Pow(1024, 2)).ToString("F0") + "MB"; }
            if (size < Math.Pow(1024, 4)) { return (size / Math.Pow(1024, 3)).ToString("F0") + "GB"; }
            if (size < Math.Pow(1024, 5)) { return (size / Math.Pow(1024, 4)).ToString("F0") + "TB"; }
            if (size < Math.Pow(1024, 6)) { return (size / Math.Pow(1024, 5)).ToString("F0") + "PB"; }
            return (size / Math.Pow(1024, 6)).ToString("F0") + "EB";
        }

        public static Stream ToStream(string str)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            //byte[] byteArray = Encoding.ASCII.GetBytes(str);
            return new MemoryStream(byteArray);
        }
        public static string ToString(Stream stream)
        {
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        //public static string GetCurrentDir()
        //{
        //    return AtiUtil.AtiUtilManager.Current.AUtilConfig.CurrentDir;
        //}

    }
}
