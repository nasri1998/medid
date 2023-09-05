using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(30)); //tambah session untuk alert

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();
app.UseSession(); //setting session
//tambahkan middleware sampai disini
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Redirect}/{action=Index}/{id?}");
//domain web utama *bisa diedit ini default // cari action indexs // cari
app.Run();
