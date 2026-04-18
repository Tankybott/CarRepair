using Microsoft.AspNetCore.Mvc;
using Model.DTOs.PortalDto;
using Model.ViewModel;

namespace CarRepair.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class OfferController : Controller
    {
        public IActionResult Index()
        {
            var vm = new OfferViewModel
            {
                Categories = GetDummyCategories()
            };

            return View(vm);
        }

        private IEnumerable<OfferServiceTypeDto> GetDummyCategories()
        {
            return new List<OfferServiceTypeDto>
            {
                // ----------------------------------------
                // 1. Tire & Wheel Services
                // ----------------------------------------
                new OfferServiceTypeDto
                {
                    Name = "Tire & Wheel Services",
                    Description = "All services related to tires, wheels, balancing and puncture repair.",
                    Services = new List<OfferServiceDto>
                    {
                        new OfferServiceDto { ShortDescription = "Tire Replacement", Description = "Replacement of all 4 tires with balancing.", EstimatedPrice = 120 },
                        new OfferServiceDto { ShortDescription = "Wheel Balancing", Description = "Precision balancing of all wheels.", EstimatedPrice = 60 },
                        new OfferServiceDto { ShortDescription = "Puncture Repair", Description = "Hot patch repair for punctured tires.", EstimatedPrice = 40 },
                        new OfferServiceDto { ShortDescription = "Seasonal Tire Change", Description = "Swap between winter and summer tires.", EstimatedPrice = 80 },
                        new OfferServiceDto { ShortDescription = "Rim Straightening", Description = "Repair and straightening of bent rims.", EstimatedPrice = 150 }
                    }
                },

                // ----------------------------------------
                // 2. Diagnostics
                // ----------------------------------------
                new OfferServiceTypeDto
                {
                    Name = "Diagnostics",
                    Description = "Computer diagnostics, electrical tests, and system checks.",
                    Services = new List<OfferServiceDto>
                    {
                        new OfferServiceDto { ShortDescription = "Computer Diagnostics", Description = "Full ECU scan and error code reading.", EstimatedPrice = 100 },
                        new OfferServiceDto { ShortDescription = "Battery Test", Description = "Load test and health check of the battery.", EstimatedPrice = 30 },
                        new OfferServiceDto { ShortDescription = "Brake System Check", Description = "Inspection of discs, pads, and brake lines.", EstimatedPrice = 50 },
                        new OfferServiceDto { ShortDescription = "Suspension Check", Description = "Shock absorber and suspension component inspection.", EstimatedPrice = 70 },
                        new OfferServiceDto { ShortDescription = "Air Conditioning Diagnostics", Description = "Leak test and performance check.", EstimatedPrice = 90 }
                    }
                },

                // ----------------------------------------
                // 3. Mechanical Repairs
                // ----------------------------------------
                new OfferServiceTypeDto
                {
                    Name = "Mechanical Repairs",
                    Description = "General mechanical repairs and maintenance.",
                    Services = new List<OfferServiceDto>
                    {
                        new OfferServiceDto { ShortDescription = "Oil Change", Description = "Engine oil and filter replacement.", EstimatedPrice = 150 },
                        new OfferServiceDto { ShortDescription = "Brake Pad Replacement", Description = "Front or rear brake pad replacement.", EstimatedPrice = 180 },
                        new OfferServiceDto { ShortDescription = "Timing Belt Replacement", Description = "Full timing kit with water pump.", EstimatedPrice = 900 },
                        new OfferServiceDto { ShortDescription = "Clutch Replacement", Description = "Complete clutch kit replacement.", EstimatedPrice = 1200 },
                        new OfferServiceDto { ShortDescription = "Alternator Replacement", Description = "Replacement of alternator and belt.", EstimatedPrice = 650 }
                    }
                },

                // ----------------------------------------
                // 4. Air Conditioning
                // ----------------------------------------
                new OfferServiceTypeDto
                {
                    Name = "Air Conditioning",
                    Description = "AC refills, leak tests, and component repairs.",
                    Services = new List<OfferServiceDto>
                    {
                        new OfferServiceDto { ShortDescription = "AC Refill", Description = "Refill with refrigerant and performance test.", EstimatedPrice = 200 },
                        new OfferServiceDto { ShortDescription = "AC Leak Detection", Description = "UV dye test for leaks.", EstimatedPrice = 120 },
                        new OfferServiceDto { ShortDescription = "Compressor Replacement", Description = "AC compressor replacement and system cleaning.", EstimatedPrice = 1500 }
                    }
                },

                // ----------------------------------------
                // 5. Electrical Services
                // ----------------------------------------
                new OfferServiceTypeDto
                {
                    Name = "Electrical Services",
                    Description = "Repairs of electrical systems, sensors, and wiring.",
                    Services = new List<OfferServiceDto>
                    {
                        new OfferServiceDto { ShortDescription = "Starter Motor Replacement", Description = "Starter motor removal and installation.", EstimatedPrice = 500 },
                        new OfferServiceDto { ShortDescription = "Sensor Replacement", Description = "Replacement of faulty sensors (ABS, lambda, etc.).", EstimatedPrice = 150 },
                        new OfferServiceDto { ShortDescription = "Lighting Repair", Description = "Headlight, taillight, and indicator repairs.", EstimatedPrice = 80 }
                    }
                },

                // ----------------------------------------
                // 6. Exhaust System
                // ----------------------------------------
                new OfferServiceTypeDto
                {
                    Name = "Exhaust System",
                    Description = "Repairs and replacement of exhaust components.",
                    Services = new List<OfferServiceDto>
                    {
                        new OfferServiceDto { ShortDescription = "Exhaust Leak Repair", Description = "Welding and sealing of exhaust leaks.", EstimatedPrice = 200 },
                        new OfferServiceDto { ShortDescription = "Catalytic Converter Replacement", Description = "Replacement of catalytic converter.", EstimatedPrice = 1800 },
                        new OfferServiceDto { ShortDescription = "Muffler Replacement", Description = "Installation of new muffler.", EstimatedPrice = 400 }
                    }
                }
            };
        }
    }
}
