using System;
using System.Data;

namespace News_Radio_app.Models;

public class Article
{
    public string Filename { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Script { get; set; } = string.Empty;

    public string genre { get; set; } = string.Empty;
    public DateTime Date { get; set; } 

    public Article()
    {

        Filename = $"{Path.GetRandomFileName()}.articles.txt";

        Title  = "Test Title";
        Url    = "Test Url";
        Script = "Test Script";
        genre  = "Test genre";

        Date = DateTime.Now;
    }

    public void Save()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, Filename);
        var lines = new[]
        {
            Title,
            Url,
            Script
        };

        File.WriteAllLines(path, lines);

        // File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename), Title);
        // File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename), Url);
        // File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename), Script);
    }
    public void Delete() =>
        File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename));


    public static Article Load(string filename)
    {
        filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        var lines = File.ReadAllLines(filename);


        return new Article
            {
                Filename = Path.GetFileName(filename),
                Title = lines.Length > 0 ? lines[0] : "",
                Url = lines.Length > 1 ? lines[1] : "",
                Script = lines.Length > 2 ? lines[2] : "",
                genre = lines.Length > 3 ? lines[3] : "",
                Date = File.GetLastWriteTime(filename)
            };
        
    }

    public static IEnumerable<Article> LoadAll()
    {
        // Get the folder where the articles are stored.
        string appDataPath = FileSystem.AppDataDirectory;

        // Use Linq extensions to load the *.articles.txt files.
        return Directory

                // Select the file names from the directory
                .EnumerateFiles(appDataPath, "*.articles.txt")

                // Each file name is used to load a article
                .Select(filename => Article.Load(Path.GetFileName(filename)))

                // With the final collection of articles, order them by date
                .OrderByDescending(article => article.Date);
    }

    //ジャンルごとのグループ一覧を取得する目的
    public static IEnumerable<IGrouping<string, Article>> LoadGroupedByGenre()
    {
        // Get the folder where the articles are stored.
        string appDataPath = FileSystem.AppDataDirectory;

        // Use Linq extensions to load the *.articles.txt files.
        return Directory

                // Select the file names from the directory
                .EnumerateFiles(appDataPath, "*.articles.txt")

                // Each file name is used to load an article
                .Select(filename => Article.Load(Path.GetFileName(filename)))

                // Group articles by genre
                .GroupBy(article => article.genre);

                // With the final collection of articles, order them by date
                // .OrderByDescending(article => article.Date);
    }

    //Genreを指定してLoadする関数
    public static IEnumerable<Article> LoadByGenre(string genre)
    {
        string appDataPath = FileSystem.AppDataDirectory;

        return Directory
                .EnumerateFiles(appDataPath, "*.articles.txt")
                .Select(filename => Article.Load(Path.GetFileName(filename)))
                .Where(article => article.genre == genre)
                .OrderByDescending(article => article.Date);
    }

}


