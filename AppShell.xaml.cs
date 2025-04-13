namespace News_Radio_app;

using Microsoft.Maui.Controls;
using System.Diagnostics; // Debug用

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(Views.NotePage), typeof(Views.NotePage));
		Routing.RegisterRoute(nameof(Views.TopPage), typeof(Views.TopPage));
		Routing.RegisterRoute(nameof(Views.ArticlePage), typeof(Views.ArticlePage));
	}

	private async void OnBackClicked(object sender, EventArgs e)
        {
			var navigation = Shell.Current?.Navigation;

			if (navigation == null || navigation.NavigationStack == null || navigation.NavigationStack.Count <= 1)
    		{
        		Debug.WriteLine("NavigationStackがないため、ページを戻ることができません");

        		return;
    		}

			var stack = navigation.NavigationStack;

            // ナビゲーション履歴をデバッグ出力
            Debug.WriteLine("=== Navigation Stack ===");
            foreach (var page in stack)
            {
                Debug.WriteLine(page.GetType().Name);
            }
            Debug.WriteLine("========================");

			// 1つ前に戻る
				if (navigation.NavigationStack.Count > 1) // 戻れる場合のみ
				{

					try
        			{
            			await navigation.PopAsync(); // 前のページに戻る
					}
					catch (Exception ex)
					{
						Debug.WriteLine($"戻る操作に失敗しました: {ex.Message}");
					}
				}

        }
}
