namespace RMS_Business
{
    /// <summary>
    /// Central registry for permission key constants used by RBAC checks.
    /// </summary>
    /// <remarks>
    /// WARNING - Security usage rules:
    /// 1) Never hardcode permission strings outside this class.
    /// 2) Never build permission keys from user input.
    /// 3) Keep keys synchronized with SQL seed file and database records.
    /// 4) If a key must change, use a database migration and update all references together.
    /// 5) Authorization checks should be deny-by-default when key resolution fails.
    /// </remarks>
    public static class clsPermissionKeys
    {
        /// <summary>
        /// Product module permissions.
        /// </summary>
        public static class Products
        {
            public const string View = "Products.View";
            public const string Create = "Products.Create";
            public const string Edit = "Products.Edit";
            public const string Delete = "Products.Delete";
            public const string Activate = "Products.Activate";
            public const string Deactivate = "Products.Deactivate";
            public const string EditImage = "Products.Image.Edit";
            public const string Export = "Products.Export";
            public const string MoveCategory = "Products.Category.Move";
        }

        /// <summary>
        /// Product unit permissions (price, conversion, and barcode controls).
        /// </summary>
        public static class ProductUnits
        {
            public const string View = "ProductUnits.View";
            public const string Create = "ProductUnits.Create";
            public const string Edit = "ProductUnits.Edit";
            public const string Delete = "ProductUnits.Delete";
            public const string EditPrice = "ProductUnits.Price.Edit";
            public const string EditConversion = "ProductUnits.Conversion.Edit";
            public const string EditBarcode = "ProductUnits.Barcode.Edit";
            public const string Activate = "ProductUnits.Activate";
            public const string Deactivate = "ProductUnits.Deactivate";
        }

        /// <summary>
        /// Global unit master-data permissions.
        /// </summary>
        public static class Units
        {
            public const string View = "Units.View";
            public const string Create = "Units.Create";
            public const string Edit = "Units.Edit";
            public const string Delete = "Units.Delete";
        }

        /// <summary>
        /// Category master-data permissions.
        /// </summary>
        public static class Categories
        {
            public const string View = "Categories.View";
            public const string Create = "Categories.Create";
            public const string Edit = "Categories.Edit";
            public const string Delete = "Categories.Delete";
        }

        /// <summary>
        /// Company permissions including search and assignment operations.
        /// </summary>
        public static class Companies
        {
            public const string View = "Companies.View";
            public const string Search = "Companies.Search";
            public const string Create = "Companies.Create";
            public const string Edit = "Companies.Edit";
            public const string Delete = "Companies.Delete";
            public const string AssignContactPerson = "Companies.ContactPerson.Assign";
            public const string AssignCountry = "Companies.Country.Assign";
            public const string Select = "Companies.Select";
        }

        /// <summary>
        /// Brand permissions including company association management.
        /// </summary>
        public static class Brands
        {
            public const string View = "Brands.View";
            public const string Create = "Brands.Create";
            public const string Edit = "Brands.Edit";
            public const string Delete = "Brands.Delete";
            public const string AssignCompany = "Brands.Company.Assign";
        }
    }
}
