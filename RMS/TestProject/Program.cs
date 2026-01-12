using System;
using System.Data;
using RMS_Business;

// Simple console harness that fails fast on the first issue, with step-by-step logging.
try
{
	
	Console.WriteLine("Running clsProduct smoke test...");
	RunProductSmoke();
	Console.WriteLine("clsProduct smoke test passed.\n");
	Environment.Exit(0);
}
catch (Exception ex)
{
	Console.Error.WriteLine($"FAIL: {ex.Message}");
	Environment.Exit(1);
}

static void RunCategorySmoke()
{
	var name = $"Test Category {Guid.NewGuid():N}";
	var description = "Added via TestProject harness";

	var category = CreateCategoryInstance(name, description, createdByUserId: null);
	Step("Save new category");
	Ensure(category.Save(), "Save(AddNew) returned false");
	Ensure(category.CategoryID > 0, "New category ID was not set");

	Step("Fetch category by ID");
	var fetched = clsCategory.Find(category.CategoryID);
	Ensure(fetched != null, "Find after add returned null");
	Ensure(string.Equals(fetched!.CategoryName, name, StringComparison.Ordinal), "CategoryName mismatch after add");
	Ensure(string.Equals(fetched.Description, description, StringComparison.Ordinal), "Description mismatch after add");

	Step("Update category fields and save");
	fetched.CategoryName = name + " updated";
	fetched.Description = "Updated via harness";
	Ensure(fetched.Save(), "Save(Update) returned false");

	Step("Fetch category after update");
	var updated = clsCategory.Find(category.CategoryID);
	Ensure(updated != null, "Find after update returned null");
	Ensure(string.Equals(updated!.CategoryName, fetched.CategoryName, StringComparison.Ordinal), "Updated CategoryName mismatch");
	Ensure(string.Equals(updated.Description, fetched.Description, StringComparison.Ordinal), "Updated Description mismatch");

	Step("List all categories");
	var all = clsCategory.GetAllCategory();
	Ensure(all != null, "GetAllCategory returned null");
	Ensure(all.Rows.Count > 0, "GetAllCategory returned zero rows");

	Step("Delete category");
	Ensure(clsCategory.DeleteCategory(category.CategoryID), "DeleteCategory returned false");
	var deleted = clsCategory.Find(category.CategoryID);
	Ensure(deleted == null, "Find after delete should be null");
}

static clsCategory CreateCategoryInstance(string name, string? description, int? createdByUserId)
{
	// clsCategory exposes only a non-public constructor; use reflection to get a fresh AddNew instance.
	var instance = (clsCategory?)Activator.CreateInstance(typeof(clsCategory), nonPublic: true);
	if (instance == null)
		throw new InvalidOperationException("Could not create clsCategory instance.");

	instance.CategoryName = name;
	instance.Description = description;
	instance.CreatedByUserID = createdByUserId;
	return instance;
}

static void RunProductSmoke()
{
	var name = $"Test Product {Guid.NewGuid():N}";
	var description = "Added via TestProject harness";

	var product = CreateProductInstance(name, categoryId: null, brandId: null, description, isActive: true, reorderLevel: 5, createdByUserId: null);
	Step("Save new product");
	Ensure(product.Save(), "Save(AddNew) for product returned false");
	Ensure(product.ProductID > 0, "New product ID was not set");

	Step("Fetch product by ID");
	var fetched = clsProduct.Find(product.ProductID);
	Ensure(fetched != null, "Find after product add returned null");
	Ensure(string.Equals(fetched!.ProductName, name, StringComparison.Ordinal), "ProductName mismatch after add");
	Ensure(string.Equals(fetched.Description, description, StringComparison.Ordinal), "Product Description mismatch after add");
	Ensure(fetched.IsActive == true, "IsActive mismatch after add");
	Ensure(fetched.ReorderLevel == 5, "ReorderLevel mismatch after add");

	Step("Update product fields and save");
	fetched.ProductName = name + " updated";
	fetched.Description = "Updated via harness";
	fetched.IsActive = false;
	fetched.ReorderLevel = 7;
	fetched.UpdatedByUserID = null; // keep null unless you want to attribute updates to a user
	Ensure(fetched.Save(), "Save(Update) for product returned false");

	Step("Fetch product after update");
	var updated = clsProduct.Find(product.ProductID);
	Ensure(updated != null, "Find after product update returned null");
	Ensure(string.Equals(updated!.ProductName, fetched.ProductName, StringComparison.Ordinal), "Updated ProductName mismatch");
	Ensure(string.Equals(updated.Description, fetched.Description, StringComparison.Ordinal), "Updated Product Description mismatch");
	Ensure(updated.IsActive == fetched.IsActive, "Updated IsActive mismatch");
	Ensure(updated.ReorderLevel == fetched.ReorderLevel, "Updated ReorderLevel mismatch");

	Step("List all products");
	var all = clsProduct.GetAllProduct();
	Ensure(all != null, "GetAllProduct returned null");
	Ensure(all.Rows.Count > 0, "GetAllProduct returned zero rows");

	Step("Delete product");
	Ensure(clsProduct.DeleteProduct(product.ProductID, UpdatedByUserID: null), "DeleteProduct returned false");
	var deleted = clsProduct.Find(product.ProductID);
	Ensure(deleted == null, "Find after product delete should be null");
}

static clsProduct CreateProductInstance(string name, int? categoryId, int? brandId, string? description, bool isActive, int reorderLevel, int? createdByUserId)
{
	// clsProduct also uses a non-public constructor; instantiate via reflection.
	var instance = (clsProduct?)Activator.CreateInstance(typeof(clsProduct), nonPublic: true);
	if (instance == null)
		throw new InvalidOperationException("Could not create clsProduct instance.");

	instance.ProductName = name;
	instance.CategoryID = categoryId;
	instance.BrandID = brandId;
	instance.Description = description;
	instance.IsActive = isActive;
	instance.ReorderLevel = reorderLevel;
	instance.CreatedByUserID = createdByUserId;
	return instance;
}

static void Ensure(bool condition, string message)
{
	if (!condition)
		throw new InvalidOperationException(message);
}

static void Step(string message)
{
	Console.WriteLine($"- {message}");
}
