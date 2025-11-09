using VelocipedeUtils.Shared.Models.Business.Processes;

namespace VelocipedeUtils.Shared.Models.Business.Cooking;

/// <summary>
/// Associate table that connects a recipe and cooking operation.
/// </summary>
public class RecipeCookingOperation : BusinessTask, IWfBusinessEntity
{
    /// <summary>
    /// Recipe.
    /// </summary>
    public Recipe? Recipe { get; set; }
    
    /// <summary>
    /// Cooking operation.
    /// </summary>
    public CookingOperation? CookingOperation { get; set; }
}