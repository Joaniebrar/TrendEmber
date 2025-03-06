using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrendEmber.Data.Migrations.TrendsDb
{
    /// <inheritdoc />
    public partial class ResistanceView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW ""ResistancePointsView"" AS
            SELECT closeEP.""Symbol"", closeEP.""Close"" AS ""Resistance"", closeEP.""PriceDate"" AS ""StartDate"", filledEP.""PriceDate"" AS ""EndDate""
            FROM public.""PriceGapEvents"" pe
            INNER JOIN public.""EquityPrices"" closeEP ON pe.""ClosingEquityPriceHistoryId"" = closeEP.""Id""
            LEFT JOIN public.""EquityPrices"" filledEP ON pe.""GapFilledPriceHistoryId"" = filledEP.""Id""
            UNION
            SELECT openEP.""Symbol"", openEP.""Open"" AS resistance, openEP.""PriceDate"" AS StartDate, filledEP.""PriceDate"" AS EndDate
            FROM public.""PriceGapEvents"" pe
            INNER JOIN public.""EquityPrices"" openEP ON pe.""OpeningEquityPriceHistoryId"" = openEP.""Id""
            LEFT JOIN public.""EquityPrices"" filledEP ON pe.""GapFilledPriceHistoryId"" = filledEP.""Id""
            UNION
            SELECT sb.""Symbol"", wp.""Price"" AS resistance, wp.""PriceDate"" AS StartDate, NULL AS EndDate
            FROM public.""WavePoints"" wp
            INNER JOIN public.""Symbols"" sb ON wp.""SymbolId"" = sb.""Id""
            UNION
            SELECT ep.""Symbol"", 
                   CASE WHEN ep.""Close"" > ep.""Open"" THEN ep.""Open""
                        ELSE ep.""Close"" 
                   END AS resistance, 
                   ep.""PriceDate"" AS StartDate, 
                   NULL AS EndDate
            FROM public.""EquityPrices"" ep
            WHERE ep.""Shape"" = 4 AND ep.""RangeZScore"" >= 1.5;
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS ResistancePointsView;");
        }
    }
}
