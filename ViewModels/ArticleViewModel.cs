using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;


namespace News_Radio_app.ViewModels;

internal class ArticleViewModel : ObservableObject, IQueryAttributable
{
  private Models.Article _article;

  public string Title
  {
    get => _article.Title;
    set
    {
      if (_article.Title != value)
      {
        _article.Title = value;
        OnPropertyChanged();
      }
    }
  }

  public string Url
  {
    get => _article.Url;
    set
    {
      if (_article.Url != value)
      {
        _article.Url = value;
        OnPropertyChanged();
      }
    }
  }

  public string Script
  {
    get => _article.Script;
    set
    {
      if (_article.Script != value)
      {
        _article.Script = value;
        OnPropertyChanged();
      }
    }
  }

  public string genre
  {
    get => _article.genre;
    set
    {
      if (_article.genre != value)
      {
        _article.genre = value;
        OnPropertyChanged();
      }
    }
  }




  public DateTime Date => _article.Date;

  public string Identifier => _article.Filename;

  public ICommand SaveCommand { get; private set; }
  public ICommand DeleteCommand { get; private set; }

  public ArticleViewModel()
  {
    _article = new Models.Article();
    SaveCommand = new AsyncRelayCommand(Save);
    DeleteCommand = new AsyncRelayCommand(Delete);
  }

  public ArticleViewModel(Models.Article article)
  {
    _article = article;
    SaveCommand = new AsyncRelayCommand(Save);
    DeleteCommand = new AsyncRelayCommand(Delete);
  }

  private async Task Save()
  {
    _article.Date = DateTime.Now;
    _article.Save();
    await Shell.Current.GoToAsync($"..?saved={_article.Filename}");
  }

  private async Task Delete()
  {
    _article.Delete();
    await Shell.Current.GoToAsync($"..?deleted={_article.Filename}");
  }

  void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
  {
    if (query.ContainsKey("load"))
    {
      _article = Models.Article.Load(query["load"].ToString());
      RefreshProperties();
    }
  }

  public void Reload()
  {
    _article = Models.Article.Load(_article.Filename);
    RefreshProperties();
  }

  private void RefreshProperties()
  {
    OnPropertyChanged(nameof(Title));
    OnPropertyChanged(nameof(Url));
    OnPropertyChanged(nameof(Script));
    OnPropertyChanged(nameof(Date));
  }
}

