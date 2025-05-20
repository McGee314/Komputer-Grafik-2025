using Godot; // This lets us use Godot's tools

public partial class GridMap : Godot.GridMap // Matches filename
{
	[Export]
	public int WorldSize = 20; // Size of the square world (same for X and Z)
	
	public override void _Ready()
	{
		var meshLibrary = MeshLibrary;
		GD.Print($"MeshLibrary assigned: {meshLibrary != null}");
		
		if (meshLibrary == null)
		{
			GD.PrintErr("ERROR: No MeshLibrary assigned to GridMap!");
			return;
		}
		
		// Look for Road item
		int roadId = meshLibrary.FindItemByName("Road");
		GD.Print($"Road tile ID: {roadId}");
		
		if (roadId >= 0)
		{
			// Print road info
			GD.Print("---- Road Details ----");
			GD.Print($"Item name: {meshLibrary.GetItemName(roadId)}");
			GD.Print($"Item mesh exists: {meshLibrary.GetItemMesh(roadId) != null}");
			
			// Generate a square world of roads
			GD.Print($"Generating SQUARE world of size {WorldSize}x{WorldSize}...");
			
			// Create a perfect square grid of road tiles
			for (int x = 0; x < WorldSize; x++)
			{
				for (int z = 0; x < WorldSize; z++)
				{
					// This creates a perfect square since x and z use the same range
					SetCellItem(new Vector3I(x, 0, z), roadId);
				}
			}
			
			GD.Print($"Square world generation complete! ({WorldSize*WorldSize} tiles placed)");
		}
		else
		{
			GD.PrintErr("ERROR: Road not found in MeshLibrary!");
			GD.Print("Available items in library:");
			
			int[] itemsList = meshLibrary.GetItemList();
			foreach (int itemId in itemsList)
			{
				string itemName = meshLibrary.GetItemName(itemId);
				GD.Print($"- Item ID: {itemId}, Name: \"{itemName}\"");
			}
		}
	}
}
