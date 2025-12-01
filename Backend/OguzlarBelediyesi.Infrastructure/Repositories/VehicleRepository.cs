using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Application;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Infrastructure.Repositories;

public sealed class VehicleRepository : IVehicleRepository
{
    private static readonly IReadOnlyList<Vehicle> Vehicles = new[]
    {
        new Vehicle(
            Id: 1,
            Name: "JCB Kazıcı Yükleyici",
            Type: "İş Makinesi",
            Plate: "19 AA 001",
            Description: "Altyapı ve kazı çalışmalarında kullanılan çok amaçlı iş makinesi.",
            ImageUrl: "assets/images/slider.jpg"
        ),
        new Vehicle(
            Id: 2,
            Name: "Ford Cargo Süpürge Kamyonu",
            Type: "Hizmet Aracı",
            Plate: "19 AA 002",
            Description: "İlçemizin temizlik hizmetlerinde kullanılan hidrolik sıkıştırmalı süpürge kamyonu.",
            ImageUrl: "assets/images/slider.jpg"
        ),
        new Vehicle(
            Id: 3,
            Name: "Mercedes-Benz İtfaiye Aracı",
            Type: "Hizmet Aracı",
            Plate: "19 AA 003",
            Description: "Yangın ve kurtarma operasyonları için tam donanımlı araç.",
            ImageUrl: "assets/images/slider.jpg"
        ),
        new Vehicle(
            Id: 4,
            Name: "Otokar Sultan Otobüs",
            Type: "Toplu Taşıma",
            Plate: "19 AA 004",
            Description: "Vatandaşlarımızın ulaşımı için kullanılan şehir içi yolcu otobüsü.",
            ImageUrl: "assets/images/slider.jpg"
        ),
        new Vehicle(
            Id: 5,
            Name: "Caterpillar Greyder",
            Type: "İş Makinesi",
            Plate: "19 AA 005",
            Description: "Yol yapım ve bakım çalışmalarında kullanılan greyder.",
            ImageUrl: "assets/images/slider.jpg"
        ),
        new Vehicle(
            Id: 6,
            Name: "Ford Transit Cenaze Aracı",
            Type: "Hizmet Aracı",
            Plate: "19 AA 006",
            Description: "Cenaze nakil hizmetlerinde kullanılan soğutuculu araç.",
            ImageUrl: "assets/images/slider.jpg"
        )
    };

    public Task<IReadOnlyList<Vehicle>> GetAllAsync()
    {
        return Task.FromResult(Vehicles);
    }
}
