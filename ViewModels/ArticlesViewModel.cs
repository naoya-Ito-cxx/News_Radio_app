using CommunityToolkit.Mvvm.Input;
using News_Radio_app.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace News_Radio_app.ViewModels;

internal class ArticlesViewModel : IQueryAttributable
{
  public string Genre {get; set; }
  public ObservableCollection<ViewModels.ArticleViewModel> Articles { get; }
  public ICommand NewCommand { get; }
  public ICommand SelectArticleCommand { get; }




  public ArticlesViewModel()
  {

    Genre = "Test Genre";

	//特定のジャンルだけ取ってくる
    // AllArticles = new ObservableCollection<ViewModels.ArticleViewModel>(Models.Article.LoadAll().Select(n => new ArticleViewModel(n)));
    Articles = new ObservableCollection<ViewModels.ArticleViewModel>(Models.Article.LoadByGenre(Genre).Select(n => new ArticleViewModel(n)));
	
    NewCommand = new AsyncRelayCommand(NewArticleAsync);
    SelectArticleCommand = new AsyncRelayCommand<ViewModels.ArticleViewModel>(SelectArticleAsync);
  }

  private async Task NewArticleAsync()
  {
    await Shell.Current.GoToAsync(nameof(Views.ArticlePage));
  }

  private async Task SelectArticleAsync(ViewModels.ArticleViewModel article)
  {
    if (article != null)
      await Shell.Current.GoToAsync($"{nameof(Views.ArticlePage)}?load={article.Identifier}");
  }

  void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
  {
    if (query.ContainsKey("deleted"))
    {
      string articleId = query["deleted"].ToString();
      ArticleViewModel matchedArticle = Articles.Where((n) => n.Identifier == articleId).FirstOrDefault();

      // If article exists, delete it
      if (matchedArticle != null)
        Articles.Remove(matchedArticle);
    }
    else if (query.ContainsKey("saved"))
    {
      string articleId = query["saved"].ToString();
      ArticleViewModel matchedArticle = Articles.Where((n) => n.Identifier == articleId).FirstOrDefault();

      // If article is found, update it
      if (matchedArticle != null)
      {
        matchedArticle.Reload();
        Articles.Move(Articles.IndexOf(matchedArticle), 0);
      }
      // If note isn't found, it's new; add it.
      else
        Articles.Insert(0, new ArticleViewModel(Models.Article.Load(articleId)));
    }
  }
}
