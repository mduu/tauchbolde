using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Tauchbolde.DataAccess;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
    }
}

public class TemporaryDbContextFactory : IDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext Create(DbContextFactoryOptions options)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=pinchdb;Trusted_Connection=True;MultipleActiveResultSets=true");
        return new ApplicationDbContext(builder.Options);
    }
}