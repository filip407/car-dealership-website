namespace CarDealership.ViewModels;

public class StatisticsViewModel
{
    public decimal TotalRevenue { get; set; }
    public int TotalCarsSold { get; set; }
    public List<AgentStatViewModel> AgentStats { get; set; } = new();
    public List<RecentSaleViewModel> RecentSales { get; set; } = new();
}

public class AgentStatViewModel
{
    public string AgentName { get; set; } = string.Empty;
    public int CarsSold { get; set; }
    public decimal Revenue { get; set; }
}

public class RecentSaleViewModel
{
    public string CarName { get; set; } = string.Empty;
    public string AgentName { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public decimal SalePrice { get; set; }
}
