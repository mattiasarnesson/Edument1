using Edument1.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Services;

namespace Edument1
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //Set the base path
            String folderURL = System.Configuration.ConfigurationManager.AppSettings["folderURL"];
            current_navigation.InnerText = System.Web.HttpContext.Current.Server.MapPath(folderURL);
        }

        /// <summary>
        /// The API for the getting some files/folders from a path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [WebMethod]
        public static String getFoldersWithBase(String path)
        {

            try
            {
                //Anti manipulation system
                if (IsSubDirectoryOf(path, System.Configuration.ConfigurationManager.AppSettings["folderURL"].ToString()))
                {
                    return jsonMessage(new MessageDelivery(System.Configuration.ConfigurationManager.AppSettings["messageDeliveryFailed"],
                        System.Configuration.ConfigurationManager.AppSettings["genericFailedMessage"]));
                }


                //Declare and create a list that will hold all the files and some information about them
                List<FileType> allFiles = new List<FileType>();

                //New list coming back
                allFiles = loadFilesAssignedFolder(allFiles, path);

                //Convert to JSON for cross-platform communication
                var json = JsonConvert.SerializeObject(allFiles);
                return json;
            }
            //Could be more specific with the error handling 
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                return jsonMessage(new MessageDelivery(System.Configuration.ConfigurationManager.AppSettings["messageDeliveryFailed"],
                    System.Configuration.ConfigurationManager.AppSettings["genericFailedMessage"]));
            }

        }

        /// <summary>
        /// Remove a file by path
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        [WebMethod]
        public static String removeFile(List<String> paths)
        {
            try
            {


                //Loop all the paths
                for (int i = 0; i < paths.Count; i++)
                {

                    //Anti manipulation system
                    if (IsSubDirectoryOf(paths[i], System.Configuration.ConfigurationManager.AppSettings["folderURL"].ToString()))
                    {
                        return jsonMessage(new MessageDelivery(System.Configuration.ConfigurationManager.AppSettings["messageDeliveryFailed"],
                            System.Configuration.ConfigurationManager.AppSettings["genericFailedMessage"]));
                    }

                    if (IsDirectory(paths[i]))
                    {
                        Directory.Delete(paths[i]);
                    }
                    else
                    {
                        File.Delete(paths[i]);
                    }

                }

                return jsonMessage(new MessageDelivery(System.Configuration.ConfigurationManager.AppSettings["messageDeliveryOk"],
                    System.Configuration.ConfigurationManager.AppSettings["removedFilesMessage"]));
            }
            //Could be more specific with the error handling 
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                return jsonMessage(new MessageDelivery(System.Configuration.ConfigurationManager.AppSettings["messageDeliveryFailed"],
                    System.Configuration.ConfigurationManager.AppSettings["genericFailedMessage"]));
            }

        }

        /// <summary>
        /// Create a file at path, by title and type
        /// </summary>
        /// <param name="path"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [WebMethod]
        public static String createFile(String path, String title, String type)
        {

            try
            {

                //Anti manipulation system
                if (IsSubDirectoryOf(path, System.Configuration.ConfigurationManager.AppSettings["folderURL"].ToString()))
                {
                    return jsonMessage(new MessageDelivery(System.Configuration.ConfigurationManager.AppSettings["messageDeliveryFailed"],
                        System.Configuration.ConfigurationManager.AppSettings["genericFailedMessage"]));
                }

                String fileName = "";

                if (type == "TXT")
                {
                    //Create a file
                    fileName = path + "\\" + title + ".txt";
                    File.Create(fileName).Close();
                }
                else if (type == "DIR")
                {
                    //Create a directory
                    fileName = path + "\\" + title;
                    Directory.CreateDirectory(fileName);
                }
                else
                {
                    return jsonMessage(new MessageDelivery(System.Configuration.ConfigurationManager.AppSettings["messageDeliveryFailed"],
                        System.Configuration.ConfigurationManager.AppSettings["notSupportedMessage"]));
                }

                return jsonMessage(new MessageDelivery(System.Configuration.ConfigurationManager.AppSettings["messageDeliveryOk"],
                    System.Configuration.ConfigurationManager.AppSettings["createdFileMessage"]));

            }

            //Could be more specific with the error handling 
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                return jsonMessage(new MessageDelivery(System.Configuration.ConfigurationManager.AppSettings["messageDeliveryFailed"],
                    System.Configuration.ConfigurationManager.AppSettings["genericFailedMessage"]));
            }

        }



        /// <summary>
        /// Reverse the path from physical path to virtual path. Used when we are navigating
        /// </summary>
        /// <param name="fullServerPath"></param>
        /// <returns></returns>
        public static string MapPathReverse(string fullServerPath)
        {
            return @"~\" + fullServerPath.Replace(HttpContext.Current.Request.PhysicalApplicationPath, String.Empty);
        }

        public static List<FileType> loadFilesAssignedFolder(List<FileType> allFiles, String path)
        {

            String folderURL;

            try
            {
                //If coming from base or has started navigation
                if (String.IsNullOrEmpty(path))
                {
                    //Get the URL from the Web.Config app Settings
                    folderURL = System.Configuration.ConfigurationManager.AppSettings["folderURL"];
                }
                else
                {
                    folderURL = MapPathReverse(path);
                }


                //Search in the directory for all the times, all search options enabled and all extensions enabled
                string[] fileArray = Directory.GetFileSystemEntries(System.Web.HttpContext.Current.Server.MapPath(folderURL), "*");

                //Loop the fileArray by path
                for (int i = 0; i < fileArray.Length; i++)
                {
                    //Create a filetype, could use constructor but going for the getters and setters, it just looks cleaner in the code
                    FileType singleFile = new FileType();

                    //We already know the path obv
                    singleFile.Path = fileArray[i];

                    //Set the extension of the file, txt, img and so on
                    singleFile.Extension = Path.GetExtension(fileArray[i]);

                    //Get the filesize(only if it is none directory)
                    if (!IsDirectory(fileArray[i]))
                    {
                        singleFile.Size = new System.IO.FileInfo(fileArray[i]).Length;
                        singleFile.Dir = false;
                    }
                    else
                    {
                        singleFile.Dir = true;

                    }


                    //Figure out a FontAwesome Icon
                    singleFile.Img = declareImageToFile(singleFile.Extension);

                    //Get the name from the file
                    singleFile.Name = Path.GetFileName(fileArray[i]);

                    //Object is now ready! Lets put it in the array allFiles
                    allFiles.Add(singleFile);

                }
            }
            //Could be more specific with the error handling 
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            } 

            return allFiles;
        }

        /// <summary>
        /// Just declaring some images to type of files
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private static string declareImageToFile(String extension)
        {
            //If or cases, just a converter
            if (extension == ".txt")
            {
                return System.Configuration.ConfigurationManager.AppSettings["iconTxt"];
            }
            else if (extension == "")
            {
                return System.Configuration.ConfigurationManager.AppSettings["iconDir"];
            }
            else
            {
                return System.Configuration.ConfigurationManager.AppSettings["iconUnknown"];
            }
        }


        /// <summary>
        /// Check if the file is a folder or not
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsDirectory(string path)
        {
            System.IO.FileAttributes fa = System.IO.File.GetAttributes(path);
            return (fa & FileAttributes.Directory) != 0;
        }

        /// <summary>
        /// Compress a message object to JSON
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static String jsonMessage(MessageDelivery message)
        {
            return JsonConvert.SerializeObject(message);
        }


        /// <summary>
        /// Anti-manipulation on the base of the folder. We must check so the user does not manipulate the JS to acsess more files then priv
        /// </summary>
        /// <param name="folderOne"></param>
        /// <param name="folderTwo"></param>
        /// <returns></returns>
        public static bool IsSubDirectoryOf(string folderOne, string folderTwo)
        {
            var child = false;


            //This is prob base case. So lets just return false
            if (String.IsNullOrEmpty(folderOne))
            {
                return false;
            }

            try
            {

                var folderOneD = new DirectoryInfo(folderOne);
                var folderTwoD = new DirectoryInfo(folderTwo);

                //Loop until break.
                while (folderOneD.Parent != null)
                {
                    if (folderOneD.Parent.FullName == folderTwoD.FullName)
                    {
                        //Break case
                        child = true;
                        break;

                    }else
                    {

                        folderOneD = folderOneD.Parent;
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to check directorys");
                Debug.WriteLine(ex);
            }

            return child;
        }

    }
}