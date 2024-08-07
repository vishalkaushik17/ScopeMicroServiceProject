"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ModuleName = exports.NodeElementType = exports.Tag = exports.ResponseStatus = void 0;
// export type StorageDataType <T extends object> =  {
//     Key: string;
//     Data: T;
//     Expiery: number;
// };
var ResponseStatus;
(function (ResponseStatus) {
    ResponseStatus["Success"] = "Success";
    ResponseStatus["Failed"] = "Failed";
})(ResponseStatus || (exports.ResponseStatus = ResponseStatus = {}));
var Tag;
(function (Tag) {
    Tag["css"] = "link";
    Tag["script"] = "script";
})(Tag || (exports.Tag = Tag = {}));
var NodeElementType;
(function (NodeElementType) {
    NodeElementType[NodeElementType["Css"] = 1] = "Css";
    NodeElementType[NodeElementType["Script"] = 2] = "Script";
})(NodeElementType || (exports.NodeElementType = NodeElementType = {}));
var ModuleName;
(function (ModuleName) {
    ModuleName["iifeIndexPageFunction"] = "iifeIndexPageFunction";
})(ModuleName || (exports.ModuleName = ModuleName = {}));
//enum User {
//    admin = "Admin",
//    user = "User"
//}
//enum UserType {
//    administrator,
//    normaluser
//}
//export type Test = {
//    id: String;
//    name: String;
//    dob: Date;
//    calculateAge: (age: number) => number; //method call signature
//    user: User
//    /*(name: string): string;*/
//    /*(age: number) :number;*/
//}
//const calculateAge = (age: number):number => {
//    return age * 1;
//}
//const temp:Test = {
//    id : "1",
//    name : "vishal",
//    dob : new Date,
//    calculateAge: (age: number) => { return age * 1; },
//    user: User.admin
//}
////tuples ex
////make sure we have to use readonly for tuples otherwise we have push additional value to that enum type.
//type PersonData = readonly [string, string, number];
//type OfficialData = readonly [string, string];
//const person: PersonData = ["vishal", "kaushik", 44];
////interface example for class
//interface IEmployee {
//    name: string;
//    getName(): string;
//}
///*class example*/
//class employee implements IEmployee {
//    name: string;
//    constructor(EmployeeName: string) {
//        this.name = EmployeeName;
//    };
//    getName():string
//    {
//        return this.name
//    }
//}
//const emp = new employee("Vishal");
//console.log(emp.getName());
///*class parent and child relationship eg*/
//class Parent {
//    name: string;
//    setName(name:string):void {
//        this.name = name;
//    }
//}
//class Child extends Parent {
//    getName():string {
//        return this.name;
//    }
//}
//let abcd = new Child();
//abcd.setName("vishal")
//console.warn(abcd.getName());
///*namespace eg*/
//namespace tempNs {
//    export class testClass {
//        name: string="vishal";
//    }
//}
//let temp1 = tempNs.testClass.name;
///// <reference path="./customTypes.ts" />
//namespace tempNs {
//    class temp2class extends testClass {
//        getName(): string {
//            return this.name;
//        }
//    }
//}
//let s1 = Symbol('s1');
//let data = {
//    [s1]:"test"
//}
//console.log(data[s1]);
