using Skclusive.Material.Layout;

namespace Xenial.Doughnut.Frontend
{
    public interface IDashboardViewConfig : ILayoutConfig
    {
    }

    public class DashboardViewConfigBuilder : LayoutConfigBuilder<DashboardViewConfigBuilder, IDashboardViewConfig>
    {
        protected class DashboardViewConfig : LayoutConfig, IDashboardViewConfig
        {
        }

        public DashboardViewConfigBuilder() : base(new DashboardViewConfig())
        {
        }

        protected override IDashboardViewConfig Config() => (IDashboardViewConfig)_config;

        protected override DashboardViewConfigBuilder Builder() => this;
    }
}
