using Android.Content;
using App5.Droid;
using System;

using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDbPath))]
namespace App5.Droid
{
    public class AndroidDbPath : IPath
    {
        public string GetSimleString()
        {
            return "Hi";
        }

        public string GetDatabasePath(string sqliteFilename)
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            // копирование файла из папки Assets по пути path
            if (!File.Exists(path))
            {
                // получаем контекст приложения
                Context context = Android.App.Application.Context;
                var dbAssetStream = context.Assets.Open(sqliteFilename);

                var dbFileStream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate);
                var buffer = new byte[1024];

                int b = buffer.Length;
                int length;

                while ((length = dbAssetStream.Read(buffer, 0, b)) > 0)
                {
                    dbFileStream.Write(buffer, 0, length);
                }

                dbFileStream.Flush();
                dbFileStream.Close();
                dbAssetStream.Close();
            }

            return path;
        }
    }
}