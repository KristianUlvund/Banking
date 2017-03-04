namespace TheBank.DAL
{
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System;
    using System.Linq;
    using System.IO;
    using System.Data.Entity.Infrastructure;
    using System.Text;

    public partial class ModelContext : DbContext
    {
        private static string fileName = "db_log.txt";
        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

        public ModelContext()
            : base("name=ModelContext")
        {
            Database.CreateIfNotExists();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();
            foreach (var entry in entries)
            {
                // If something was added to the database
                if (entry.State == EntityState.Added)
                {
                    List<string> propManager    = new List<string>();
                    List<string> newValues      = new List<string>();

                    foreach (string o in entry.CurrentValues.PropertyNames)
                    {
                        var property = entry.Property(o);

                        propManager.Add(property.Name);

                        var currentValue = property.CurrentValue;
                        if(currentValue.GetType() == typeof(DateTime))
                        {
                            newValues.Add(((DateTime)currentValue).ToShortDateString());
                        }
                        else
                        {
                            newValues.Add(currentValue.ToString());
                        }
                    }
                    logToFile(propManager, null, newValues);
                }

                // If something was modified
                else if (entry.State == EntityState.Modified)
                {
                    List<string> propManager = new List<string>();
                    List<string> newValues = new List<string>();
                    List<string> original = new List<string>();

                    foreach (string o in entry.OriginalValues.PropertyNames)
                    {
                        var property = entry.Property(o);

                        propManager.Add(property.Name);
                        var currentValue = property.CurrentValue;
                        if (currentValue.GetType() == typeof(DateTime))
                        {
                            newValues.Add(((DateTime)currentValue).ToShortDateString());
                        }
                        else
                        {
                            newValues.Add(currentValue.ToString());
                        }

                        var originalValue = property.OriginalValue;
                        if (originalValue.GetType() == typeof(DateTime))
                        {
                            original.Add(((DateTime)originalValue).ToShortDateString());
                        }
                        else
                        {
                            original.Add(originalValue.ToString());
                        }
                    }
                    logToFile(propManager, original, newValues);
                }

                // Something is deleted
                else
                {
                    List<string> propManager = new List<string>();
                    List<string> original = new List<string>();

                    foreach (string o in entry.OriginalValues.PropertyNames)
                    {
                        var property = entry.Property(o);

                        propManager.Add(property.Name);
                        var originalValue = property.OriginalValue;
                        if (originalValue.GetType() == typeof(DateTime))
                        {
                            original.Add(((DateTime)originalValue).ToShortDateString());
                        }
                        else
                        {
                            original.Add(originalValue.ToString());
                        }
                    }
                    logToFile(propManager, original, null);
                }
            }
            return base.SaveChanges();
        }

        private void logToFile(List<string> prop, List<string> original, List<string> newValues)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                string tab = "\t\t\t";
                writer.WriteLine();
                writer.WriteLine("Date: " + DateTime.Now);
                writer.WriteLine();
                // We start by writing the property names in a file.
                foreach (string p in prop)
                {
                    writer.Write(p + tab);
                }
                writer.WriteLine();

                // If the original values are present, log them
                if (original != null)
                {
                    foreach (string o in original)
                    {
                        writer.Write(o + tab);
                    }
                    writer.WriteLine("(Original values)");
                }

                // If new values are present, log them
                if (newValues != null)
                {
                    foreach (string n in newValues)
                    {
                        writer.Write(n + tab);
                    }
                    writer.WriteLine("(New values)");
                }

                writer.WriteLine("----------------------------------------------");
                writer.WriteLine();
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Postal> Postals { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Admin> Admins { get; set; }


        /// <summary>
        /// The user will represent the customer.
        /// </summary>
        public class User
        {
            [Key]
            public string personalIdentification { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string address { get; set; }
            public string phoneNumber { get; set; }
            public string email { get; set; }
            public byte[] password { get; set; }
            public string salt { get; set; }
            public bool active { get; set; }

            public virtual Postal postalCode { get; set; }
            public virtual List<Account> accounts { get; set; }
        }

        public class Admin
        {
            [Key]
            public string personalIdentification { get; set; }
        }

        public class Account
        {
            [Key]
            public int aID { get; set; }
            public string accountNumber { get; set; }
            public string name { get; set; }
            public double balance { get; set; }
            public bool active { get; set; }

            public virtual AccountType accountType { get; set; }

            public static implicit operator Account(string v)
            {
                throw new NotImplementedException();
            }
        }

        public class Transaction
        {
            [Key]
            public int tID { get; set; }
            public DateTime date { get; set; }
            public double amount { get; set; }
            public string message { get; set; }
            public bool isTransferred { get; set; }

            public virtual Account fromAccount { get; set;}
            public virtual Account toAccount { get; set; }
        }

        public class Postal
        {
            [Key]
            public string postalCode { get; set; }
            public string city { get; set; }
        }


        public class AccountType
        {
            [Key]
            public int accTypeID { get; set; }
            public bool card { get; set; }
            public double interest { get; set; }
            public double limit { get; set; }
            public double yearlyFee { get; set; }
        }
    }
}
