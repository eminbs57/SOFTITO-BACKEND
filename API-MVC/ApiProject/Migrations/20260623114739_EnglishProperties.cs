using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiProject.Migrations
{
    /// <inheritdoc />
    public partial class EnglishProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Plaka",
                table: "Vehicles",
                newName: "VehicleType");

            migrationBuilder.RenameColumn(
                name: "Marka",
                table: "Vehicles",
                newName: "LicensePlate");

            migrationBuilder.RenameColumn(
                name: "Kapasite",
                table: "Vehicles",
                newName: "Capacity");

            migrationBuilder.RenameColumn(
                name: "AracTipi",
                table: "Vehicles",
                newName: "Brand");

            migrationBuilder.RenameColumn(
                name: "Telefon",
                table: "Drivers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Soyad",
                table: "Drivers",
                newName: "LicenseClass");

            migrationBuilder.RenameColumn(
                name: "EhliyetSinifi",
                table: "Drivers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Ad",
                table: "Drivers",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Sehir",
                table: "Depots",
                newName: "DepotName");

            migrationBuilder.RenameColumn(
                name: "Hacim",
                table: "Depots",
                newName: "Volume");

            migrationBuilder.RenameColumn(
                name: "DepoAdi",
                table: "Depots",
                newName: "City");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleType",
                table: "Vehicles",
                newName: "Plaka");

            migrationBuilder.RenameColumn(
                name: "LicensePlate",
                table: "Vehicles",
                newName: "Marka");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "Vehicles",
                newName: "Kapasite");

            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "Vehicles",
                newName: "AracTipi");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Drivers",
                newName: "Telefon");

            migrationBuilder.RenameColumn(
                name: "LicenseClass",
                table: "Drivers",
                newName: "Soyad");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Drivers",
                newName: "EhliyetSinifi");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Drivers",
                newName: "Ad");

            migrationBuilder.RenameColumn(
                name: "Volume",
                table: "Depots",
                newName: "Hacim");

            migrationBuilder.RenameColumn(
                name: "DepotName",
                table: "Depots",
                newName: "Sehir");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Depots",
                newName: "DepoAdi");
        }
    }
}
