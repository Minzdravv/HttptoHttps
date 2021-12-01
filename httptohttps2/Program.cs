using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections.Specialized;

// Само по себе лучше поструктурированее 

namespace httptohttps2
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string directoryout = ConfigurationManager.AppSettings.Get("DirectoryOut");
            string filemask = ConfigurationManager.AppSettings.Get("FileMask");
            string directory = ConfigurationManager.AppSettings.Get("DirectoryRun");
            
            ReplaceInFile(directoryout, Directory.GetFiles(directory , filemask), "http://", "https://");
        }

        // Эта функция много откуда вызываться может, они должна быть доступной для других методов класса, не только 'Main( )'
        static void ReplaceInFile(string directory, string[] files, string searchText, string replaceText)
        {
        
            foreach( string file in files )
            {
                // Уведомляем в консоль
                Console.WriteLine(file);

                // Я точно не помню, но кажется в с# могут быть утечки ресурсов
                //  поэтому IDisposable объекты в using желательно заворачивать
                //  https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement

                // Ну и вообще ниже всё разбито на понятные логические блоки, а то концов не найдёшь потом
                // И комментим всё


                // Переменная для загрузки файла для обработки
                string content;

                // Читаем
                using( StreamReader reader = new StreamReader(file) ) 
                {
                    content = reader.ReadToEnd();
                    //reader.Close(); // Не факт что эта строка вообще нужна, выше Using
                }

                // Заменяем
                content = Regex.Replace(content, searchText, replaceText);

                // Пишем
                string fileout = directory + file;
                using( StreamWriter writer = new StreamWriter(fileout) )
                {
                    writer.Write(content);
                    //writer.Close(); // Не факт что эта строка вообще нужна, выше Using
                }
            }

        }

    }
}
