namespace TestApi.Models
{
    public class SelectViewModel
    {
        public SelectViewModel(){}

        public SelectViewModel(int id, string text)
        {
            Id = id;
            Text = text;
        }

        public int Id { get; set; }
        public string Text { get; set; }
    }
}
