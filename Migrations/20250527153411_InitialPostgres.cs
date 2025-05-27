using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VehicleAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuelSupplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MarcaTemporal = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlacasDelVehiculo = table.Column<string>(type: "text", nullable: true),
                    TipoCombustible = table.Column<string>(type: "text", nullable: true),
                    Kilometraje = table.Column<int>(type: "integer", nullable: true),
                    CantidadGalones = table.Column<decimal>(type: "numeric(10,3)", nullable: true),
                    ValorCombustible = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    DiligenciadoPor = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelSupplies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MarcaTemporal = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Placa = table.Column<string>(type: "text", nullable: true),
                    NombreConductor = table.Column<string>(type: "text", nullable: true),
                    TarjetaPropiedad = table.Column<string>(type: "text", nullable: true),
                    Soat = table.Column<string>(type: "text", nullable: true),
                    SeguridadSocial = table.Column<string>(type: "text", nullable: true),
                    LicenciaConduccion = table.Column<string>(type: "text", nullable: true),
                    CertificadoRevision = table.Column<string>(type: "text", nullable: true),
                    FechaVencimientoLicencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PerteneceEmpresa = table.Column<string>(type: "text", nullable: true),
                    FechaVencimientoSoat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaVencimientoRevision = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EntregaRecibe = table.Column<string>(type: "text", nullable: true),
                    FotoFrontalVehiculo = table.Column<string>(type: "text", nullable: true),
                    FotoLateralDerechoVehiculo = table.Column<string>(type: "text", nullable: true),
                    FotoLateralIzquierdoVehiculo = table.Column<string>(type: "text", nullable: true),
                    FotoTraseraVehiculo = table.Column<string>(type: "text", nullable: true),
                    CarroceriaLatoneriaPuertas = table.Column<string>(type: "text", nullable: true),
                    CarroceriaMarcoCarcasaMotos = table.Column<string>(type: "text", nullable: true),
                    CarroceriaSuspension = table.Column<string>(type: "text", nullable: true),
                    CarroceriaAsientosCojineria = table.Column<string>(type: "text", nullable: true),
                    CarroceriaSegurosBloqueosPuertas = table.Column<string>(type: "text", nullable: true),
                    CarroceriaLimpiabrisas = table.Column<string>(type: "text", nullable: true),
                    CarroceriaVidrios = table.Column<string>(type: "text", nullable: true),
                    CarroceriaRetrovisores = table.Column<string>(type: "text", nullable: true),
                    ElementosLabradoLlantas = table.Column<string>(type: "text", nullable: true),
                    ElementosPresionLlantas = table.Column<string>(type: "text", nullable: true),
                    ElementosNivelLiquidos = table.Column<string>(type: "text", nullable: true),
                    ElementosAusenciaFugas = table.Column<string>(type: "text", nullable: true),
                    ElementosPedales = table.Column<string>(type: "text", nullable: true),
                    ElementosFrenosDiscoMotos = table.Column<string>(type: "text", nullable: true),
                    ElementosCadenaMotos = table.Column<string>(type: "text", nullable: true),
                    ElementosManijasFrenoClutchMotos = table.Column<string>(type: "text", nullable: true),
                    ElectricoFarolas = table.Column<string>(type: "text", nullable: true),
                    ElectricoDireccionalesParqueo = table.Column<string>(type: "text", nullable: true),
                    ElectricoLucesReversa = table.Column<string>(type: "text", nullable: true),
                    ElectricoLucesFrenoPare = table.Column<string>(type: "text", nullable: true),
                    ElectricoIndicadoresTablero = table.Column<string>(type: "text", nullable: true),
                    ElectricoPito = table.Column<string>(type: "text", nullable: true),
                    ElectricoAlarmaReversa = table.Column<string>(type: "text", nullable: true),
                    PrevencionCinturonesSeguridad = table.Column<string>(type: "text", nullable: true),
                    PrevencionAlarmaSeguridad = table.Column<string>(type: "text", nullable: true),
                    PrevencionKitCarretera = table.Column<string>(type: "text", nullable: true),
                    PrevencionExtintor = table.Column<string>(type: "text", nullable: true),
                    PrevencionLlantaRepuesto = table.Column<string>(type: "text", nullable: true),
                    PrevencionChalecoReflectivo = table.Column<string>(type: "text", nullable: true),
                    PrevencionCasco = table.Column<string>(type: "text", nullable: true),
                    VehiculoAutorizadoTransitar = table.Column<string>(type: "text", nullable: true),
                    FactoresImpidenMovilizacion = table.Column<string>(type: "text", nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    NombreConductorCC = table.Column<string>(type: "text", nullable: true),
                    NombreResponsableVehiculoCC = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registros", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelSupplies");

            migrationBuilder.DropTable(
                name: "Registros");
        }
    }
}
