using MongoDB.Driver;
using System.Configuration;
namespace file
{
    public partial class Form1 : Form
    {
        IMongoCollection<product> productCollection;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase(databaseName);
            productCollection = database.GetCollection<product>("product");
            LoadProductData();
        }
        private void LoadProductData()
        {
            var filterDefinition = Builders<product>.Filter.Empty;
            var products = productCollection.Find(filterDefinition).ToList();
            dataGridView1.DataSource = products;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            var product = new product
            {
                ProductCode = txtProductCode.Text,
                ProductName = txtProductName.Text,
                Price = decimal.Parse(txtPrice.Text)
            };
            productCollection.InsertOne(product);
            LoadProductData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var filterDefinition = Builders<product>.Filter.Eq(a => a.ProductCode, txtProductCode.Text);
            productCollection.DeleteOne(filterDefinition);
            LoadProductData();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var filterDefinition = Builders<product>.Filter.Eq(a => a.ProductCode, txtProductCode.Text);
            var updateDefinition = Builders<product>.Update
                .Set(a => a.ProductName, txtProductName.Text)
                .Set(a => a.Price, decimal.Parse(txtPrice.Text));
            productCollection.UpdateOne(filterDefinition, updateDefinition);
            LoadProductData();

        }
    }
}
