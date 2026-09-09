using BureauOrdreDigital.Api.Modules.Courrier.Models;
using BureauOrdreDigital.Api.Shared.Data;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BureauOrdreDigital.Api.Modules.Courrier.Services;

public class OrganismeExcelImporter
{
    private readonly AppDbContext _context;

    public OrganismeExcelImporter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> ImporterAsync(string cheminFichier)
    {
        if (!File.Exists(cheminFichier))
        {
            throw new FileNotFoundException(
                $"Le fichier Excel est introuvable : {cheminFichier}");
        }

        using var workbook = new XLWorkbook(cheminFichier);

        // =========================================================
        // FEUILLE À IMPORTER
        // =========================================================

        if (!workbook.Worksheets.Contains("Feuil1"))
        {
            throw new Exception(
                "La feuille 'Feuil1' est introuvable dans le fichier Excel.");
        }

        var worksheet = workbook.Worksheet("Feuil1");

        // =========================================================
        // LECTURE DES EN-TÊTES
        // =========================================================

        var headerRow = worksheet.FirstRowUsed();

        if (headerRow == null)
        {
            throw new Exception("La feuille Excel est vide.");
        }

        var headers = new Dictionary<string, int>(
            StringComparer.OrdinalIgnoreCase);

        foreach (var cell in headerRow.CellsUsed())
        {
            var header = cell.GetString().Trim();

            if (!string.IsNullOrWhiteSpace(header))
            {
                headers[header] = cell.Address.ColumnNumber;
            }
        }

        // =========================================================
        // VÉRIFICATION DES COLONNES OBLIGATOIRES
        // =========================================================

        string[] colonnesObligatoires =
        {
            "Nom du Client",
            "Nom Abrégé",
            "Type",
            "Pays",
            "Scope",
            "Mode",
            "Marché",
            "Partenaire",
            "Contrat"
        };

        foreach (var colonne in colonnesObligatoires)
        {
            if (!headers.ContainsKey(colonne))
            {
                throw new Exception(
                    $"La colonne obligatoire '{colonne}' est absente du fichier Excel.");
            }
        }

        // =========================================================
        // TRANSACTION
        // =========================================================

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            int nombreImporte = 0;

            var firstDataRow = headerRow.RowNumber() + 1;
            var lastDataRow = worksheet.LastRowUsed()?.RowNumber()
                              ?? firstDataRow - 1;

            // =====================================================
            // LECTURE DES 105 LIGNES
            // =====================================================

            for (int rowNumber = firstDataRow;
                 rowNumber <= lastDataRow;
                 rowNumber++)
            {
                var row = worksheet.Row(rowNumber);

                // Ligne complètement vide => on ignore
                if (row.CellsUsed().All(c =>
                        string.IsNullOrWhiteSpace(c.GetString())))
                {
                    continue;
                }

                var nom = LireTexte(
                    worksheet,
                    rowNumber,
                    headers,
                    "Nom du Client");

                // Nom obligatoire
                if (string.IsNullOrWhiteSpace(nom))
                {
                    continue;
                }

                var organisme = new Organisme
                {
                    Nom = nom,

                    NomAbrege = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Nom Abrégé"),

                    Type = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Type"),

                    Pays = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Pays"),

                    // Le fichier Excel actuel ne contient pas
                    // de colonne Ville.
                    Ville = LireTexteOptionnel(
                        worksheet,
                        rowNumber,
                        headers,
                        "Ville"),

                    Scope = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Scope"),

                    Mode = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Mode"),

                    Marche = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Marché"),

                    Partenaire = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Partenaire"),

                    Contrat = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Contrat"),

                    DateAnniversaire = LireDate(
                        worksheet,
                        rowNumber,
                        headers,
                        "Date Anniversaire"),

                    VersionElise = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Version Elise"),

                    NombreUtilisateurs = LireInt(
                        worksheet,
                        rowNumber,
                        headers,
                        "Nombre Utilisateurs"),

                    MontantContrat = LireDecimal(
                        worksheet,
                        rowNumber,
                        headers,
                        "Montant Contrat"),

                    ChefProjetCsm = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Chef Projet/CSM"),

                    CodeDivalto = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Code Divalto"),

                    NomContact = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Nom Contact"),

                    MailContact = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Mail Contact"),

                    TelContact = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Tel Contact"),

                    FonctionContact = LireTexte(
                        worksheet,
                        rowNumber,
                        headers,
                        "Fonction Contact")
                };

                // =================================================
                // ACTIF
                // =================================================
                //
                // Ton Excel actuel n'a pas de colonne "Actif".
                // On utilise donc "Contrat".
                //
                // Contrat = Actif   => Actif = true
                // Contrat = Inactif => Actif = false
                //
                // Si une colonne "Actif" existe dans un futur Excel,
                // elle sera utilisée directement.
                // =================================================

                if (headers.ContainsKey("Actif"))
                {
                    organisme.Actif = LireBoolean(
                        worksheet,
                        rowNumber,
                        headers,
                        "Actif");
                }
                else
                {
                    var contrat = organisme.Contrat.Trim();

                    organisme.Actif =
                        contrat.Equals(
                            "Actif",
                            StringComparison.OrdinalIgnoreCase);
                }

                // =================================================
                // IMPORTANT :
                // PAS DE RECHERCHE D'UN ORGANISME EXISTANT
                // PAS DE DÉDUPLICATION
                //
                // Chaque ligne Excel devient une nouvelle ligne SQL.
                // =================================================

                _context.Organismes.Add(organisme);

                nombreImporte++;
            }

            // =====================================================
            // SAUVEGARDE
            // =====================================================

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return nombreImporte;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // =============================================================
    // LIRE TEXTE
    // =============================================================

    private static string LireTexte(
        IXLWorksheet worksheet,
        int rowNumber,
        Dictionary<string, int> headers,
        string colonne)
    {
        if (!headers.TryGetValue(colonne, out var columnNumber))
        {
            return string.Empty;
        }

        return worksheet
            .Cell(rowNumber, columnNumber)
            .GetString()
            .Trim();
    }

    // =============================================================
    // LIRE TEXTE OPTIONNEL
    // =============================================================

    private static string LireTexteOptionnel(
        IXLWorksheet worksheet,
        int rowNumber,
        Dictionary<string, int> headers,
        string colonne)
    {
        if (!headers.ContainsKey(colonne))
        {
            return string.Empty;
        }

        return LireTexte(
            worksheet,
            rowNumber,
            headers,
            colonne);
    }

    // =============================================================
    // LIRE INT
    // =============================================================

    private static int? LireInt(
        IXLWorksheet worksheet,
        int rowNumber,
        Dictionary<string, int> headers,
        string colonne)
    {
        if (!headers.TryGetValue(colonne, out var columnNumber))
        {
            return null;
        }

        var cell = worksheet.Cell(rowNumber, columnNumber);

        if (cell.IsEmpty())
        {
            return null;
        }

        if (cell.TryGetValue<int>(out var intValue))
        {
            return intValue;
        }

        var texte = cell.GetString().Trim();

        if (int.TryParse(
                texte,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var result))
        {
            return result;
        }

        if (int.TryParse(
                texte,
                NumberStyles.Integer,
                CultureInfo.GetCultureInfo("fr-FR"),
                out result))
        {
            return result;
        }

        return null;
    }

    // =============================================================
    // LIRE DECIMAL
    // =============================================================

    private static decimal? LireDecimal(
        IXLWorksheet worksheet,
        int rowNumber,
        Dictionary<string, int> headers,
        string colonne)
    {
        if (!headers.TryGetValue(colonne, out var columnNumber))
        {
            return null;
        }

        var cell = worksheet.Cell(rowNumber, columnNumber);

        if (cell.IsEmpty())
        {
            return null;
        }

        if (cell.TryGetValue<decimal>(out var decimalValue))
        {
            return decimalValue;
        }

        var texte = cell.GetString().Trim();

        if (decimal.TryParse(
                texte,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var result))
        {
            return result;
        }

        if (decimal.TryParse(
                texte,
                NumberStyles.Any,
                CultureInfo.GetCultureInfo("fr-FR"),
                out result))
        {
            return result;
        }

        return null;
    }

    // =============================================================
    // LIRE DATE
    // =============================================================

    private static DateTime? LireDate(
        IXLWorksheet worksheet,
        int rowNumber,
        Dictionary<string, int> headers,
        string colonne)
    {
        if (!headers.TryGetValue(colonne, out var columnNumber))
        {
            return null;
        }

        var cell = worksheet.Cell(rowNumber, columnNumber);

        if (cell.IsEmpty())
        {
            return null;
        }

        if (cell.TryGetValue<DateTime>(out var dateValue))
        {
            return dateValue;
        }

        var texte = cell.GetString().Trim();

        string[] formats =
        {
            "dd/MM/yyyy",
            "dd/MM/yyyy HH:mm:ss",
            "MM/dd/yyyy",
            "yyyy-MM-dd",
            "dd-MM-yyyy"
        };

        if (DateTime.TryParseExact(
                texte,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var result))
        {
            return result;
        }

        if (DateTime.TryParse(
                texte,
                CultureInfo.GetCultureInfo("fr-FR"),
                DateTimeStyles.None,
                out result))
        {
            return result;
        }

        return null;
    }

    // =============================================================
    // LIRE BOOLEAN
    // =============================================================

    private static bool LireBoolean(
        IXLWorksheet worksheet,
        int rowNumber,
        Dictionary<string, int> headers,
        string colonne)
    {
        if (!headers.TryGetValue(colonne, out var columnNumber))
        {
            return false;
        }

        var texte = worksheet
            .Cell(rowNumber, columnNumber)
            .GetString()
            .Trim();

        return texte.Equals(
                   "Actif",
                   StringComparison.OrdinalIgnoreCase)
               || texte.Equals(
                   "true",
                   StringComparison.OrdinalIgnoreCase)
               || texte.Equals("1");
    }
}