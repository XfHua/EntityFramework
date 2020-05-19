using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App243
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            ApplicationContext context = new ApplicationContext();
            context.Database.EnsureCreated();


            Task.Run (async () => {

                var newPosts = new Setting() { Key = "123", Value = "test" };
                context.Setting.Add(newPosts);


                var result = context.Setting.Where(X => X.Key == "123").FirstOrDefault();

                Console.WriteLine(result.Value);
            } );


        }
    }

    public class ApplicationContext : DbContext
    {
        private const string databaseName = "Database.db";
        public DbSet<Setting> Setting { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String databasePath = "";
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    SQLitePCL.Batteries_V2.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", databaseName); ;
                    break;
                case Device.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);
                    break;
                case Device.WPF:
                    {
                        databasePath = Path.Combine(Environment.CurrentDirectory, databaseName);
                        break;
                    }
                default:
                    throw new NotImplementedException("Platform not supported");
            }
            if (!File.Exists(databasePath))
            {
                File.Create(databasePath);
            }
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }
    }
    public class Setting
    {
        [Key]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
