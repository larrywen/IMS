

at razor page,
@Inject IViewInventoriesByNameUseCase ViewInventoriesByNameUseCase



private List<Inventory>? inventories;
protected override Task OnInitializeAsync()
{
	//return base.OnInitializeAsync();
	inventories = (await ViewInventoriesByNameUseCase.ExeciuteAsync()).ToList();
}


21. SPA Componentd Best Practice
Break down component for reusable

27.
@inject NavigationManager NavigationManager
<EditForm Model="inventory" FormName="formInventory" OnValidSubmit="Save">
    <DataAnnotationsValidator></DataAnnotationsValidator>
    <ValidationSummary></ValidationSummary>
    <div class="form-group">
        <label for="name">Inventory Name</label>
        <InputText
        id="name"
        @bind-Value="inventory.InventoryName"
        class="form-control">
        </InputText>
        <ValidationMessage For="() => inventory.InventoryName"></ValidationMessage>
    </div>
	<br/>
    <button type="submit" class="btn btn-primary">Save</button>
    &nbsp;
    <a href="/inventories" class="btn btn-primary">Cancel</a>
</EditForm>

@code {
    [SupplyParameterFromForm] //!!!
    private Inventory inventory { get; set; } = new Inventory();

    private async Task Save()
    {
        await AddInventoryUseCase.ExecuteAsync(inventory);
        NavigationManager.NavigateTo("/inventories");
    }
}




33. Implement View Inventory Use Case and Repository
        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            return await Task.FromResult(_inventories.First(x => x.InventoryId == inventoryId));
        }
		
		
	C:\CodeUdemy\IMS\IMS.WebApp\Components\Pages\Inventories\EditInventory.razor	
	        <InputNumber @bind-Value="inventory.InventoryId" hidden></InputNumber>
			
			
    [SupplyParameterFromForm]
    private Inventory? inventory { get; set; } //declare as property in order to use [SupplyParameterFromForm] attribute

    protected override async Task OnParametersSetAsync()
    {
        this.inventory ??= await ViewInventoryByIdUseCase.ExecuteAsync(this.InvId);
        //if this.inventory has value, will not call ********************
    }		
		
.NE 10
NavigationManager.NotFound();	
		


38. Use EventCallback to communicate from child to parent component
EventCallback, from child back to parent

C:\CodeUdemy\IMS\IMS.WebApp\Components\Controls\SearchComponent.razor
@if(searchFilter is null)
{
    searchFilter = string.Empty;
}

<EditForm Enhance="true" Model="searchFilter" FormName="formSearch" OnSubmit="Search">
    <div class="input-group">
        <InputText class="form-control"
                   placeholder="Type something to search" 
                   @bind-Value="this.searchFilter">
        </InputText>
        <button type="submit" class="btn-success input-group-text">Search</button>
    </div>

</EditForm>

@code {
    [SupplyParameterFromForm]                                                                                      
    private string searchFilter { get; set; } = string.Empty;
    
    [Parameter]
    public EventCallback<string> OnSearch { get; set; }

    private void Search()
    {
        //invoke the event callback
        OnSearch.InvokeAsync(searchFilter);
    }
}


C:\CodeUdemy\IMS\IMS.WebApp\Components\Pages\Inventories\InventoryList.razor
@page "/inventories"

<h3>Inventory List</h3>
<br/>
<br/>

<SearchComponent OnSearch="HandleSearch"></SearchComponent>
<br />

<InventoryListComponent SearchInventoryFilter="@inventoryNameToSearch"></InventoryListComponent>
<br />
<a href="/addinventory" class="btn btn-primary">Add Inventory</a>

@code {
    private string? inventoryNameToSearch;

    private void HandleSearch(string searchFilter)
    {
        inventoryNameToSearch = searchFilter;
    }
}


C:\CodeUdemy\IMS\IMS.WebApp\Components\Controls\InventoryListComponent.razor
@inject IViewInventoriesByNameUseCase ViewInventoriesByNameUseCase

@if (inventories is not null && inventories.Count > 0)
{
    <table class="table">
        <thead>
            <tr>
                <th>Name</th>
                <th>Quantity</th>
                <th>Price</th>
                <th></th>
                <th style="text-align:left"></th>
            </tr>
        </thead>
        <tbody>
            @foreach (var inv in inventories)
            {
                <InventoryListItemComponent Inventory="inv"></InventoryListItemComponent>
            }
        </tbody>
    </table>
    }

@code {
    private List<Inventory>? inventories;

    [Parameter]
    public string? SearchInventoryFilter { get; set; }
    /*
    protected override async Task OnInitializedAsync()  //happens first
    {
        inventories = (await ViewInventoriesByNameUseCase.ExecuteAsync(SearchInventoryFilter??"")).ToList();
    }
    */
    protected override async Task OnParametersSetAsync() //triggered when a parameter assigned
    {
        inventories = (await ViewInventoriesByNameUseCase.ExecuteAsync(SearchInventoryFilter ?? string.Empty)).ToList();
    }
}


