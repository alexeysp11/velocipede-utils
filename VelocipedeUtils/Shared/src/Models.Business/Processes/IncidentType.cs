using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.Processes;

/// <summary>
/// Incident type.
/// </summary>
public enum IncidentType
{
    [Display(Name = "Accident")]
    Accident,
    
    [Display(Name = "Injury")]
    Injury,
    
    [Display(Name = "Event")]
    Event,
    
    [Display(Name = "Resource problem")]
    ResourceProblem,
    
    [Display(Name = "Communication problem")]
    CommunicationProblem,
    
    [Display(Name = "Equipment problem")]
    EquipmentProblem,
    
    [Display(Name = "Interference")]
    Interference,
    
    [Display(Name = "Robbery")]
    Robbery,
    
    [Display(Name = "Product damage")]
    ProductDamage, 
    
    [Display(Name = "Theft")]
    Theft,

    [Display(Name = "Service access")]
    ServiceAccess,
    
    [Display(Name = "Incorrect calculation")]
    IncorrectCalculation,
    
    [Display(Name = "Slow loading")]
    SlowLoading,
    
    [Display(Name = "User experience")]
    UserExperience,
    
    [Display(Name = "Server down")]
    ServerDown,
    
    [Display(Name = "Power line down")]
    PowerLineDown,

    [Display(Name = "Traffic accident")]
    TrafficAccident,
    
    [Display(Name = "Traffic event")]
    TrafficEvent,
    
    [Display(Name = "Transfer of vehicles")]
    TransferOfVehicles,

    [Display(Name = "Quality of pavement")]
    QualityOfPavement,
    
    [Display(Name = "Intense rainfall")]
    IntenseRainfall,
    
    [Display(Name = "Snow")]
    Snow, 
    
    [Display(Name = "Intense snow")]
    IntenseSnow, 

    [Display(Name = "Cancelled En Route")]
    CancelledEnRoute,
    
    [Display(Name = "Unable to locate")]
    UnableToLocate,

    [Display(Name = "Building fire")]
    BuildingFire,
    
    [Display(Name = "Vehicle fire")]
    VehicleFire,
    
    [Display(Name = "Cooking fire")]
    CookingFire,
    
    [Display(Name = "Wildland fire")]
    WildlandFire,
    
    [Display(Name = "Brush fire")]
    BrushFire,
    
    [Display(Name = "Grass fire")]
    GrassFire,
    
    [Display(Name = "Unauthorized burning")]
    UnauthorizedBurning,

    [Display(Name = "Search for person on land")]
    SearchForPersonOnLand,
    
    [Display(Name = "Search for person on water")]
    SearchForPersonOnWater,
    
    [Display(Name = "Ice rescue")]
    IceRescue,
    
    [Display(Name = "Lock-in")]
    LockIn,

    [Display(Name = "Technical failure")]
    TechnicalFailure,
    
    [Display(Name = "Lack of vehicles")]
    LackOfVehicles,
    
    [Display(Name = "Driver's fault")]
    DriversFault,
    
    [Display(Name = "Operational fault")]
    OperationalFault,
    
    [Display(Name = "Planning")]
    Planning,

    [Display(Name = "Other")]
    Other
}