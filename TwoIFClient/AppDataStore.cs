using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using OtpLibrary;
using TwoIFClient;

public static class AppDataStore
{
    public static void Save(string fileName, OtpDatabase database, string password)
    {
        SaveImplementation(fileName, false, database, password);
    }

    public static void SaveHeaderOnly(string fileName, OtpDatabase database)
    {
        SaveImplementation(fileName,true,database);
    }

    private static void SaveImplementation(string fileName, bool headerOnly, OtpDatabase database, string password = null)
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
            if(headerOnly)
            {
                database.WriteOnlyHeaderToFile(tempPath, actualPath);
            }
            else
            {
                database.WriteToFile(tempPath, password);
            }

            if (File.Exists(actualPath))
            {
                File.Move(actualPath, oldPath);
            }
            File.Move(tempPath, actualPath);
            if (File.Exists(oldPath))
            {
                OverriteDelete(oldPath);
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

    [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
    private static void OverriteDelete(string filePath)
    {
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.None))
        {
            int blockSize = 1024;
            while(blockSize > fileStream.Length && blockSize > 1)
            {
                blockSize /= 2;
            }

            fileStream.Position = 0L;
            byte[] dataToFill = new byte[blockSize];
            byte[] extraData = new byte[1];

            while (fileStream.Position < (fileStream.Length-blockSize))
            {
                rng.GetBytes(dataToFill);
                fileStream.Write(dataToFill,0,dataToFill.Length);

            }
            fileStream.Flush();

            while (fileStream.Position < fileStream.Length)
            {
                rng.GetBytes(extraData);
                fileStream.Write(extraData, 0, extraData.Length);
            }
            fileStream.Flush();
        }

        File.Delete(filePath);
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
