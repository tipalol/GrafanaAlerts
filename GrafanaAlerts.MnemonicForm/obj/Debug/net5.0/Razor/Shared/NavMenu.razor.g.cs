#pragma checksum "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/Shared/NavMenu.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0570c9bc40c260ae1a2a9f42d4a14b29a76fc60b"
// <auto-generated/>
#pragma warning disable 1591
namespace GrafanaAlerts.MnemonicForm.Shared
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/_Imports.razor"
using GrafanaAlerts.MnemonicForm;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/_Imports.razor"
using GrafanaAlerts.MnemonicForm.Shared;

#line default
#line hidden
#nullable disable
    public partial class NavMenu : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "top-row navbar navbar-dark");
            __builder.AddAttribute(2, "style", "padding-left: 0.8rem !important;");
            __builder.AddAttribute(3, "b-6kwv4lczuj");
            __builder.AddMarkupContent(4, "<a class=\"navbar-brand\" href b-6kwv4lczuj><div style=\"background-image: url(\'img/morty-icon-16.png\'); background-position: center; background-size: cover; height: 60px; width: 60px;\" b-6kwv4lczuj></div></a>\n    ");
            __builder.OpenElement(5, "button");
            __builder.AddAttribute(6, "class", "navbar-toggler");
            __builder.AddAttribute(7, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 5 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/Shared/NavMenu.razor"
                                             ToggleNavMenu

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(8, "b-6kwv4lczuj");
            __builder.AddMarkupContent(9, "<span class=\"navbar-toggler-icon\" b-6kwv4lczuj></span>");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(10, "\n\n");
            __builder.OpenElement(11, "div");
            __builder.AddAttribute(12, "class", 
#nullable restore
#line 10 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/Shared/NavMenu.razor"
             NavMenuCssClass

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(13, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 10 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/Shared/NavMenu.razor"
                                        ToggleNavMenu

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(14, "b-6kwv4lczuj");
            __builder.OpenElement(15, "ul");
            __builder.AddAttribute(16, "class", "nav flex-column");
            __builder.AddAttribute(17, "b-6kwv4lczuj");
            __builder.OpenElement(18, "li");
            __builder.AddAttribute(19, "class", "nav-item px-3 menu-item");
            __builder.AddAttribute(20, "b-6kwv4lczuj");
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Routing.NavLink>(21);
            __builder.AddAttribute(22, "class", "nav-link");
            __builder.AddAttribute(23, "href", "");
            __builder.AddAttribute(24, "Match", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.Routing.NavLinkMatch>(
#nullable restore
#line 13 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/Shared/NavMenu.razor"
                                                     NavLinkMatch.All

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(25, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(26, "<span class=\"oi oi-home\" aria-hidden=\"true\" b-6kwv4lczuj></span>");
            }
            ));
            __builder.CloseComponent();
            __builder.CloseElement();
            __builder.AddMarkupContent(27, "\n         ");
            __builder.OpenElement(28, "li");
            __builder.AddAttribute(29, "class", "nav-item px-3 menu-item");
            __builder.AddAttribute(30, "b-6kwv4lczuj");
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Routing.NavLink>(31);
            __builder.AddAttribute(32, "class", "nav-link");
            __builder.AddAttribute(33, "href", "http://inside.beeline.ru/");
            __builder.AddAttribute(34, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(35, "<div style=\"width: 50px; height: 50px; margin-top:  30px; background-size: 100%; background-repeat: no-repeat; background-image: url(\'img/grafana.png\')\" b-6kwv4lczuj></div>");
            }
            ));
            __builder.CloseComponent();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 26 "/Users/tipalol/RiderProjects/GrafanaAlerts/GrafanaAlerts.MnemonicForm/Shared/NavMenu.razor"
       
    private bool collapseNavMenu = true;

    private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }


#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
