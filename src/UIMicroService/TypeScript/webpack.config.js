const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const HtmlBundlerPlugin = require('html-bundler-webpack-plugin');

const mode = process.env.NODE_ENV === "production" ? "production" : "development"; 

module.exports={
    //  entry:'./main.ts',
    mode: mode,
    
    entry:{
        main            : './main.ts',
        apimethod       : './ApiMethods/ApiMethod.ts',
        tsstorage       : './BrowserStorage/tsStorage.ts',
        customtypes     : './CustomType/customTypes.ts',
        asidebar        : './Modules/asideBar.ts',
        modulefunctions     : './Modules/ModuleFunctions.ts',
        homeindex   : './Modules/Dashboard/homeindex.ts',
        SchoolLibraryIndex     : './Modules/Library/SchoolLibraryIndex.ts',
        authorindex     : './Modules/Library/AuthorIndex.ts',
        languageindex   : './Modules/General/LanguageIndex.ts',
        currencyindex   : './Modules/General/currencyindex.ts',
        utils     : './Security/utils.ts',
        identitycontroller     : './Security/IdentityController.ts',
        spalink    : './UIComponents/spalink.ts',
        spamodulelist   : './UIComponents/SPAModuleList.ts',
        productindex     : './Modules/InventoryModule/ProductIndex.ts',
        vendorindex     : './Modules/InventoryModule/VendorIndex.ts',
        employeeindex   : './Modules/EmployeeModule/employeeindex.ts',
        bankindex   : './Modules/EmployeeModule/bankindex.ts',
        designationindex   : './Modules/EmployeeModule/designationindex.ts',
        degreeindex   : './Modules/EmployeeModule/degreeindex.ts',
        departmentindex   : './Modules/EmployeeModule/departmentindex.ts',
        employeequalificationindex   : './Modules/EmployeeModule/employeequalificationindex.ts',
        },
        // plugins: [
        //     new HtmlBundlerPlugin({
        //       entry: './', // the template directory
        //     }),
        //   ],
    //entry:'./Modules/Library/SchoolLibrary.ts',
    output:{
         filename:'scripts/[name].min.js',
        //filename:'schoollibrary.js',
        // path: path.resolve(__dirname,'../wwwroot/dist')
        path :path.resolve(__dirname,'../wwwroot/dist'),
        clean: false,
        publicPath: ""
    },
   
    devtool:'inline-source-map',
    module:{
        rules:[
            {
                test:/\.ts$/,
                use:'ts-loader',
                exclude:/node_modules/
            }
        ]
    },
    resolve:{
        extensions:['.ts','.js']
    }
};