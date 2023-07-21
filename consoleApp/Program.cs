using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace consoleApp
{
    class Program
    {
        interface IProductDal
        {
            int Count();
            List<Product> GetAll();
            Product GetById(int id);
            List<Product> Find(string Name);
            int Add(Product product);
            int Update(Product product);
            int Delete(int id);
        }
        public class MySqlProductDal : IProductDal
        {
            private MySqlConnection GetMySqlConnection()
            {
                string connectionString = @"server=localhost;port=3306;database=shopapp;user=root;password=Erenjr;";
                return new MySqlConnection(connectionString);

            }
            public int Add(Product product)
            {
                int result = 0;
                using (var connection = GetMySqlConnection())
                {
                    try
                    {
                        connection.Open();
                        Console.WriteLine("Connection Opened");
                        string sql = "insert into products (Name,Price,ImageUrl,Category) VALUES (@Name,@Price,@ImageUrl,@Category)";
                        MySqlCommand command = new MySqlCommand(sql, connection);

                        command.Parameters.Add("@Name", MySqlDbType.String).Value = product.Name;
                        command.Parameters.Add("@Price", MySqlDbType.Int32).Value = product.Price;
                        command.Parameters.Add("@ImageUrl", MySqlDbType.String).Value = product.ImageUrl;
                        command.Parameters.Add("@Category", MySqlDbType.String).Value = product.Category;

                        result = command.ExecuteNonQuery();
                        Console.WriteLine($"{result} adet kayıt eklendi.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                        Console.WriteLine("Connection Closed");
                    }
                    return result;
                }
            }

            public int Delete(int id)
            {
               int result=0;
               using (var connection = GetMySqlConnection())
               {
                   try
                   {
                       connection.Open();
                       Console.WriteLine("Connection Opened");
                       string sql = "delete from products where Id=@Id";
                       MySqlCommand command = new MySqlCommand(sql, connection);
                       command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                       result = command.ExecuteNonQuery();
                       Console.WriteLine($"{result} adet kayıt silindi.");
                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine(ex.Message);
                   }
                   finally
                   {
                       connection.Close();
                       Console.WriteLine("Connection Closed");
                   }
               }
                return result;
            }

            public List<Product> GetAll()
            {
                List<Product> products = null;
                using (var connection = GetMySqlConnection())
                {
                    try
                    {
                        connection.Open();
                        Console.WriteLine("Connection Opened");
                        string sql = "select * from products";
                        MySqlCommand command = new MySqlCommand(sql, connection);
                        MySqlDataReader reader = command.ExecuteReader();

                        products = new List<Product>();
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                Name = reader["Name"].ToString(),
                                Price = Convert.ToInt32(reader["Price"].ToString()),
                                ImageUrl = reader["ImageUrl"]?.ToString(),
                                Category = reader["Category"]?.ToString()
                            });
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                        Console.WriteLine("Connection Closed");
                    }
                }
                return products;
            }

            public Product GetById(int id)
            {
                Product product = null;
                using (var connection = GetMySqlConnection())
                {
                    try
                    {
                        connection.Open();
                        Console.WriteLine("Connection Opened");

                        string sql = "select * from products where Id=@Id";
                        MySqlCommand command = new MySqlCommand(sql, connection);
                        command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                        MySqlDataReader reader = command.ExecuteReader();

                        reader.Read();
                        if (reader.HasRows)
                        {
                            product = new Product()
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                Name = reader["Name"].ToString(),
                                Price = Convert.ToInt32(reader["Price"].ToString()),
                                ImageUrl = reader["ImageUrl"]?.ToString(),
                                Category = reader["Category"]?.ToString()
                            };
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                        Console.WriteLine("Connection Closed");
                    }
                }
                return product;
            }

            public int Update(Product product)
            {
                int result = 0;
                using (var connection = GetMySqlConnection())
                {
                    try
                    {
                        connection.Open();
                        string sql = "update products set Name=@Name,Price=@Price,ImageUrl=@ImageUrl,Category=@Category where Id=@Id";
                        MySqlCommand command = new MySqlCommand(sql, connection);

                        command.Parameters.Add("@Name", MySqlDbType.String).Value = product.Name;
                        command.Parameters.Add("@Price", MySqlDbType.Int32).Value = product.Price;
                        command.Parameters.Add("@ImageUrl", MySqlDbType.String).Value = product.ImageUrl;
                        command.Parameters.Add("@Category", MySqlDbType.String).Value = product.Category;
                        command.Parameters.Add("@Id", MySqlDbType.Int32).Value = product.Id;

                        result = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return result;
            }

            public List<Product> Find(string Name)
            {
                List<Product> products = null;
                using (var connection = GetMySqlConnection())
                {
                    try
                    {
                        connection.Open();
                        Console.WriteLine("Connection Opened");

                        string sql = "select * from products where Name LIKE @Name";
                        MySqlCommand command = new MySqlCommand(sql, connection);
                        command.Parameters.Add("@Name", MySqlDbType.String).Value = "%" + Name + "%";
                        MySqlDataReader reader = command.ExecuteReader();

                        products = new List<Product>();
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                Name = reader["Name"].ToString(),
                                Price = Convert.ToInt32(reader["Price"].ToString()),
                                ImageUrl = reader["ImageUrl"]?.ToString(),
                                Category = reader["Category"]?.ToString()
                            });
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                        Console.WriteLine("Connection Closed");
                    }
                }
                return products;
            }

            public int Count()
            {
                int count = 0;
                using (var connection = GetMySqlConnection())
                {
                    try
                    {
                        connection.Open();
                        Console.WriteLine("Connection Opened");

                        string sql = "select count(*) from products";
                        MySqlCommand command = new MySqlCommand(sql, connection);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            count = Convert.ToInt32(result);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                        Console.WriteLine("Connection Closed");
                    }
                }
                return count;
            }
        }

        static void Main(string[] args)
        {
            var productDal = new MySqlProductDal();
            // var products = productDal.GetAll();
            // foreach (var item in products)
            // {
            //     Console.WriteLine($"Id: {item.Id} Name: {item.Name} Price: {item.Price} Category: {item.Category}");
            // }
            // var product = productDal.GetById(2);
            // Console.WriteLine($"{product.Name}");

            // var products2 = productDal.Find("Iphone");
            // foreach (var item in products2)
            // {
            //     Console.WriteLine($"Id:{item.Id} Name:{item.Name} Price:{item.Price} Category:{item.Category}");

            // }
            // int count = productDal.Count();
            // Console.WriteLine($"Total Products:{count}");
            // var p = new Product()
            // {
            //     Name = "Samsung S11",
            //     Price = 5500,
            //     ImageUrl = "samsungS11.jpg",
            //     Category = "Telefon"
            // };
            // productDal.Add(p);

            // var p2 = new Product()
            // {
            //     Id = 2,
            //     Name = "Samsung S7",
            //     Price = 3500,
            //     ImageUrl = "samsungS7.jpg",
            //     Category = "Telefon"
            // };
            // int count = productDal.Update(p2);
            // Console.WriteLine($"Güncellenen Kayıt Sayısı:{count}");







            // var p = productDal.GetById(2);
            // p.Name = "Samsung S8";
            // p.Price = 4500;
            // p.ImageUrl = "samsungS8.jpg";
            // p.Category = "Telefon";
            // int count = productDal.Update(p);
            // Console.WriteLine($"Güncellenen Kayıt Sayısı:{count}");


            // var products = productDal.Delete(8);
           


        }




    }
}
