namespace PetStore.Common
{
    public static class CategoryInputModelValidationConstants
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 50;

        public const string NameLengthErrorMessage =
            "Category name must be between 3 and 50 characters long!";
    }
}
