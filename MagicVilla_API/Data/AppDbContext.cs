using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
                :base(options) 
        {                           
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Villa>().HasData(
                    new Villa()
                    {
                        Id = 1,
                        Name = "Villa Lumi",
                        Details = "The beautiful Lumi villa is located not far from the city of Zadar and is an excellent choice for a pleasant family vacation on the Adriatic coast. Stylish design and luxurious interior are the main features of this home.\r\n\r\nThis newly built house consists of ground floor and first floor - on the spacious ground floor of the open plan there is a fully equipped kitchen, a cozy living room and a dining room with 10 chairs. On the ground floor there is also a bedroom with a double bed and an en-suite bathroom, an entertainment room and a separate toilet. On the first floor there are 3 more bedrooms with comfortable double beds and en suite bathrooms. Regarding entertainment, the villa offers billiards, table football, darts and PlayStation 4. Kids will have fun playing on the slides, swings and in sandboxes. All rooms are air-conditioned and covered with wireless internet.\r\nOn a 6000 m2 plot there is a heated pool of 40 m2 surrounded by 6 sun loungers where you can relax and lounge. ",
                        ImageURL = "https://www.croatialuxuryrent.com/storage/upload/646/f5b/b00/thumb-bd266b25bc2b36c17e3267dc6d8ba531-villa%20LUMI,%20Murvica%20-%202.jpg",
                        Occupancy = 10,
                        Rate = 270,
                        SquareFoot = 260,
                        Amenity = "",
                        CreatedDate = DateTime.Now,
                    },
                    new Villa()
                    {
                        Id = 2,
                        Name = "VILLA HOUSE OF POSEIDON",
                        Details = "Villa House of Poseidon located only some twenty meters from the sea and the beach in Petrčane, ten kilometers from Zadar, owes its name to the Greek sea god. The house is built in 2021, completely inspired by the sea, from its rooms with elements of blue marble depicting the turbulent sea surface all the way to the cinema, which is an exceptional experience of diving into the seabed. This new, modern, low-energy house is located on a plot of 500m2, while its inside area of ​​320m2 provides superior luxury for 8 guests and can accommodate a total of 10.\r\n\r\nVilla has exceptional micro-location in well built and maintained settlement, close to the sea and a position neighboring the natural park (no neighbors on the west, only green “wall”). Guests can expect inspiring house design and green surroundings, at the same time only fey meters from diving/swimming into the Adriatic Sea at well arranged public beaches suitable for kids.",
                        ImageURL = "https://www.croatialuxuryrent.com/storage/upload/60a/bf3/6be/thumb-75e1727e5db33129dd5c2fdd3a9afb3d-IMG_5654_tn.jpg",
                        Occupancy = 10,
                        Rate = 556,
                        SquareFoot = 320,
                        Amenity = "",
                        CreatedDate = DateTime.Now,
                    },
                    new Villa()
                    {
                        Id = 3,
                        Name = "Villa La Vrana",
                        Details = "Villa La Vrana is located in a beautiful place near the Vrana Lake Nature Park. The unique location of this facility will leave you breathless with its picturesque views of Vrana Lake and the Adriatic Sea.\r\n\r\nVilla La Vrana in its 130m2 can accommodate 8 adults in 3 modern bedrooms. On the first floor of the villa you can find two bedrooms equipped with double beds and one harmoniously decorated bathroom with bathtub. On the ground floor there is a luxuriously decorated living room with a beautiful view of the surroundings. The fully equipped kitchen and dining room of the villa is the perfect place in the villa for a family meal. In the living room there is a sofa that serves as an extra bed for two people. One room on the ground floor is also available to our guests with a double bed and a bathroom equipped with a shower. In the courtyard of the villa you can find a beautiful heated pool of 32m2 surrounded by 6 deck chairs. The whole land is completely surrounded by a wall 2 meters high. Of the additional facilities, our guests have at their disposal 3 parking spaces, a children's playground and an outdoor barbecue perfect for family gatherings or dinner with friends.",
                        ImageURL = "https://www.croatialuxuryrent.com/storage/upload/629/df9/af6/thumb-cb81b2a3e9e01abee0fe67029baafe9e-DSC_7796.jpg",
                        Occupancy = 8,
                        Rate = 233,
                        SquareFoot = 130,
                        Amenity = "",
                        CreatedDate = DateTime.Now,
                    }
                    );
        }
    }
}
