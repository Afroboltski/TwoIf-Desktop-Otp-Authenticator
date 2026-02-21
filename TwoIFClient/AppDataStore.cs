using System;
using System.IO;
using System.Reflection;
using OtpLibrary;
using TwoIFClient;

public static class AppDataStore
{
    public static void Save(string fileName, OtpDatabase database, string password)
    {
        string dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            Assembly.GetExecutingAssembly().GetName().Name);

        Directory.CreateDirectory(dir);

        string tempPath = Path.Combine(dir, Path.GetFileNameWithoutExtension(fileName) + "_tmp" + Path.GetExtension(fileName));
        string oldPath = Path.Combine(dir, Path.GetFileNameWithoutExtension(fileName) + "_old" + Path.GetExtension(fileName));
        string actualPath = Path.Combine(dir, fileName);

        try
        {
            database.WriteToFile(tempPath, password);
            if (File.Exists(actualPath))
            {
                File.Move(actualPath, oldPath);
            }
            File.Move(tempPath, actualPath);
            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }
        }
        catch (Exception ex)
        {
            if (!File.Exists(actualPath) && File.Exists(oldPath))
            {
                File.Move(oldPath, actualPath);
            }
            throw;
        }
    }

    public static OtpDatabase Load(string fileName, string password)
    {

        string path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            Assembly.GetExecutingAssembly().GetName().Name,
            fileName);
        if(!File.Exists(path))
        {
            return null;
        }

        OtpDatabase database = OtpDatabase.LoadFromFile(path, password);
        if(database == null)
        {
            throw new InvalidPasswordException("Failed to decrypt database - invalid password.");
        }
        return database;
    }

    public static bool IsPresent(string fileName)
    {
        string path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            Assembly.GetExecutingAssembly().GetName().Name,
            fileName);
        return File.Exists(path);
    }
}
