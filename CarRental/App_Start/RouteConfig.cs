using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarRental
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("", "Stronaglowna",
                defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapRoute("", "Samochod/{carBrand}-{carModel}",
                defaults: new { controller = "Home", action = "CarDetails", carBrand = UrlParameter.Optional, carModel = UrlParameter.Optional }
            );
            routes.MapRoute("", "PotwierdzRezerwacje/{carBrand}-{carModel}",
                defaults: new { controller = "Home", action = "ConfirmRezervation", carBrand = UrlParameter.Optional, carModel = UrlParameter.Optional }
            );
            routes.MapRoute("", "SamochodyDoWynajmu/Strona{page}",
                defaults: new { controller = "Home", action = "FreeCars", page = 1}
            );
            routes.MapRoute("", "Logowanie",
                defaults: new { controller = "Account", action = "Login" }
            );
            routes.MapRoute("", "Rejestracja",
                defaults: new { controller = "Account", action = "Register" }
            );
            routes.MapRoute("", "MojeKonto/MojeDane",
                defaults: new { controller = "Account", action = "AccountData" }
            );
            routes.MapRoute("", "MojeKonto/MojeDane/ZmienHaslo",
                defaults: new { controller = "Account", action = "ChangePassword" }
            );
            routes.MapRoute("", "MojeKonto/WynajeteSamochody/Aktualne/Strona{page}",
            defaults: new { controller = "Account", action = "RentedCars", past = false, page = 1 }
            );
            routes.MapRoute("", "MojeKonto/WynajeteSamochody/Zakonczone/Strona{page}",
                defaults: new { controller = "Account", action = "RentedCars", past = true, page = 1 }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
