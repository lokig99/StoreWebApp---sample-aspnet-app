## WebStoreApp - sample app made with ASP.NET MVC

This is a simple app made strictly for learning purposes.

Technologies used in this project are as follows:

- ASP.NET MVC
- Razor Pages
- Entity Framework
- Newtonsoft.Json

Other: session and cookies services

### Demo

#### Articles CRUD page

Page for managing data saved in database.

![Crud](./readme/crud.png?raw=true 'CRUD page')

#### Adding new item page

Simple form supporting fields validation and uploading images to the server.

![addding new item](./readme/adding_item.png?raw=true 'Adding new item')

#### Store page with products list filtered by category

Previous category choice is remembered by session variable. Items can be added to shopping cart by clicking "shopping cart" button.

![store page](./readme/store.png?raw=true 'store page')

#### Shopping cart page

Shopping cart is remembered using cookies and storing data serialized to JSON format in those cookies.

![cart page](./readme/cart1.png?raw=true 'cart page')

Shopping cart also support adding and removing items using "basket bin" button or "+" and "-" buttons.

![cart page - deleting product](./readme/cart2.png?raw=true 'cart remove')
