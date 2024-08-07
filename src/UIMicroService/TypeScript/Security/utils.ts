import { defineDriver } from "localforage";
import * as _type from "../CustomType/customTypes";

export const cipher = salt => {
  const textToChars = text => text.split('').map(c => c.charCodeAt(0));
  const byteHex = n => ("0" + Number(n).toString(16)).substr(-2);
  const applySaltToChar = code => textToChars(salt).reduce((a, b) => a ^ b, code);

  return text => text.split('')
    .map(textToChars)
    .map(applySaltToChar)
    .map(byteHex)
    .join('');
}

export const decipher = salt => {
  const textToChars = text => text.split('').map(c => c.charCodeAt(0));
  const applySaltToChar = code => textToChars(salt).reduce((a, b) => a ^ b, code);
  return encoded => encoded.match(/.{1,2}/g)
    .map(hex => parseInt(hex, 16))
    .map(applySaltToChar)
    .map(charCode => String.fromCharCode(charCode))
    .join('');
}

export class GenericFunctionality {
  static getFormattedDate(date) {
    const day = date.getDate();
    const month = date.getMonth() + 1;
    const year = date.getFullYear().toString().slice(2);
    return day + '-' + month + '-' + year;
  }

  /**
   * Copy to clipboard element's text children without html TAG name.
   * @param ele element which is getting copied.
   */
  static CopyToClipboard(ele) {
    let r = document.createRange();
    r.selectNode(ele as HTMLElement)!;
    window!.getSelection()!.removeAllRanges();
    window.getSelection()!.addRange(r);
    document!.execCommand('copy');
    window.getSelection()!.removeAllRanges();
  }

  /**
   * Resize Image
   * @param file File.
   * @returns void.
   */
  static resizeImage(file: File) {
    const reader = new FileReader();

    reader.onload = function (event) {
      const img = new Image();

      img.onload = function () {
        const canvas = document.getElementById('canvas')! as HTMLCanvasElement;
        const ctx = canvas.getContext('2d')!;

        const maxWidth = 1024;
        const maxHeight = 1024;
        let width = img.width;
        let height = img.height;

        if (width > maxWidth || height > maxHeight) {
          if (width > height) {
            height = Math.round((maxWidth / width) * height);
            width = maxWidth;
          } else {
            width = Math.round((maxHeight / height) * width);
            height = maxHeight;
          }
        }

        canvas.width = width;
        canvas.height = height;
        ctx.drawImage(img, 0, 0, width, height);

        // Convert canvas to Blob and create a new file
        canvas.toBlob(function (blob) {
          const resizedFile = new File([blob!], file.name, {
            type: file.type,
            lastModified: Date.now()
          });

          console.log('Resized file:', resizedFile);

          // Now you can upload the resized file to your server or perform other actions
        }, file.type);
      };

      img!.src = event.target!.result as string;
    };

    reader.readAsDataURL(file);
  }
  /**
 * Recursive method for converting object to FormData.
 * @param formdata FormData type.
 * @param data object type.
 * @param parentKey string type.
 * @returns void.
 */
  static appendFormData(formData: FormData, data: object, parentKey?: string) {

    if (data && typeof data === 'object' && !(data instanceof File)) {
      Object.keys(data).forEach(key => {
        GenericFunctionality.appendFormData(formData, data[key], parentKey ? `${parentKey}[${key}]` : key);
      });
    } else {
      parentKey = parentKey! as string;
      formData.append(parentKey, data);
    }

  }
  // static obj2FormData(obj, formDataIn = new FormData()): FormData {

  //   let formData: FormData = formDataIn;

  //   let createFormData: any = function (obj, subKeyStr = '') {
  //     for (let i in obj) {
  //       //let value:unknown = obj[i]! ;
  //       let subKeyStrTrans = subKeyStr ? subKeyStr + '[' + i + ']' : i;

  //       if (typeof (obj[i]) === 'string' || typeof (obj[i]) === 'number') {

  //         formData.append(subKeyStrTrans, String(obj[i]));

  //       } else if (typeof (obj[i]) === 'object') {

  //         createFormData(obj[i], subKeyStrTrans);

  //       }
  //     }
  //   }

  //   createFormData(obj);

  //   return formData;
  // }
  /**
   * To nullify elements value on form before submiting to api which are not required.
   * @param objectToSubmitForm object type.
   * @param element HTMLElement type.
   * @param key string type.
   * @param value object type.
   * @returns void.
   */
  static removeNotRequiredProperties(objectToSubmitForm: object, element: HTMLElement, key: string,value: _type.Primitive) {

    if (element && element.hasAttribute('dontcount') == true) {
      objectToSubmitForm[key] = null
    }
    else {
      //objectToSubmitForm[key] = value;
      if (typeof value === 'string') {
        objectToSubmitForm[key] = value as string;
      } else if (typeof value === 'boolean') {
        objectToSubmitForm[key] = value as boolean;
      } else if (typeof value === 'number') {
        objectToSubmitForm[key] = value as number;
      } else {
        if (value !== null){
          objectToSubmitForm[key] = value;
        }else{
          objectToSubmitForm[key] = null;
        }
      }
    }
  }

  
  /**
 * To Create Nested Object.
 * @param obj object type.
 * @param path HTMLElement name with or without . character.
 * @param value object type.
 * @returns void.
 */
  // static CreateNestedObject(obj: object, path: string, value: object | string | boolean | number ) {

  //   const keys = path.split('.');
  //   let current = obj;
  //   for (let i = 0; i < keys.length - 1; i++) {
  //     if (!current[keys[i]]) {
  //       current[keys[i]] = {};
  //     }
  //     current = current[keys[i]];
  //   }
  //   current[keys[keys.length - 1]] = value;
  // }

  static CreateNestedObject(obj: object, path: string, value: _type.Primitive) {
    const keys = path.split('.');
    let current: any = obj;
  
    for (let i = 0; i < keys.length - 1; i++) {
      if (!current[keys[i]]) {
        current[keys[i]] = {};
      }
      current = current[keys[i]];
    }
  
    const finalKey = keys[keys.length - 1];
  
    if (typeof value === 'string') {
      current[finalKey] = value as string;
    } else if (typeof value === 'boolean') {
      current[finalKey] = value as boolean;
    } else if (typeof value === 'number') {
      current[finalKey] = value as number;
    } else {
      current[finalKey] = value;
    }
  }

  /**
* To select dropdown option selected.
* @param selectObj HTMLSelectElement type.
* @param valueToSet number/string/object type.
* @returns void.
*/
  static SetSelectedValue(selectObj: HTMLSelectElement, valueToSet: number | string | object) {
    for (var i = 0; i < selectObj.options.length; i++) {
      if (selectObj.options[i].value == valueToSet) {
        selectObj.options[i].selected = true;
        selectObj.selectedIndex = i;
        GenericFunctionality.HtmlLogs(`SetSelectedValue`, `Option value selected!`, selectObj.options[i].text);
        return;
      }
    }
  }
  static SetSelectedValueByText(selectObj: HTMLSelectElement, valueToSet) {
    for (var i = 0; i < selectObj.options.length; i++) {
      if (selectObj.options[i].text == valueToSet) {
        selectObj.options[i].selected = true;
        selectObj.selectedIndex = i;
        GenericFunctionality.HtmlLogs(`SetSelectedValueByText`, `Option value selected!`, selectObj.options[i].text);
        return;
      }
    }
  }

  //get form element key values
  static getFormElementKeyValuePairs(form: FormData) {
    GenericFunctionality.HtmlLogs(`getFormElementKeyValuePairs -`, `Getting key value pair of form element`, '')
    let formKeyValuePairElement: _type.FormKeyValuePairElement[] = [];
    [...form].forEach((singleElement) => {
      formKeyValuePairElement.push({ key: singleElement[0].toUpperCase(), actualKey: singleElement[0], value: singleElement[1], htmlElement: singleElement, })
    })
    GenericFunctionality.HtmlLogs(`getFormElementKeyValuePairs -`, `Key Value pair`, formKeyValuePairElement)
    return formKeyValuePairElement;
  }
  static prettyJ(json) {
    if (typeof json !== 'string') {
      json = JSON.stringify(json, undefined, 2);
    }
    return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g,
      function (match) {
        let cls = "\x1b[36m";
        if (/^"/.test(match)) {
          if (/:$/.test(match)) {
            cls = "\x1b[34m";
          } else {
            cls = "\x1b[32m";
          }
        } else if (/true|false/.test(match)) {
          cls = "\x1b[35m";
        } else if (/null/.test(match)) {
          cls = "\x1b[31m";
        }
        return cls + match + "\x1b[0m";
      }
    );
  }
  static syntaxHighlight(json) {

    if (typeof json != 'string') {
      json = JSON.stringify(json, undefined, 2);
    }
    json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function (match) {
      var cls = 'number';
      if (/^"/.test(match)) {
        if (/:$/.test(match)) {
          cls = 'key';
        } else {
          cls = 'string';
        }
      } else if (/true|false/.test(match)) {
        cls = 'boolean';
      } else if (/null/.test(match)) {
        cls = 'null';
      }
      // console.log( '<span class="' + cls + '">' + match + '</span>');
      return '<span class="' + cls + '">' + match + '</span>';
    });
  }
  static HtmlApiLogs(message: string, apiLogs?: string) {
    if (GenericFunctionality.Environment && GenericFunctionality.Environment == _type.Environment.Production) {
      return;
    }

    let logsModal = document.querySelector(`#logs`)! as HTMLDivElement;
    logsModal.innerHTML += `<b style='color:#574fa9;'>TS Operation:</b> (${new Date().toLocaleString()}): ${message}: <pre class='displaylogs'>${apiLogs}</pre> <br>`;
  }
  //setting setup environment to handle trace operations and others which are dependent on SetupEnvironment
  public static Environment: _type.Environment;
  /**
   * Add item to cookie
   * @param key key for cookie item
   * @param value  value of cookie.
   * @param expirationDays expiration in days.
   */
  static addItemToCookie(key: string, value: string, expirationDays: number) {
    const date = new Date();
    date.setTime(date.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
    const expires = "; expires=" + date.toUTCString();
    document.cookie = key + "=" + value + expires + "; path=/";
  }

  /**
   * Get Cookie value from browser cookie.
   * @param key key to fetch cookie from browser
   * @returns
   */
  static getCookie(key: string): string | undefined {
    const name = key + "=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookieArray = decodedCookie.split(';');

    for (let i = 0; i < cookieArray.length; i++) {
      let cookie = cookieArray[i];
      while (cookie.charAt(0) === ' ') {
        cookie = cookie.substring(1);
      }
      if (cookie.indexOf(name) === 0) {
        return cookie.substring(name.length, cookie.length);
      }
    }
    return undefined;
  }
  /**
 * Add execution log on Log modal
 * @param message Message to display on log title.
 * @param key Message key.
 * @param dataToDisplay object type data
 * @returns void.
 */
  static HtmlLogs(message: string, key?: string, dataToDisplay?: any): void {
    if (GenericFunctionality.Environment && GenericFunctionality.Environment == "Production") {
      return;
    }
    let logsModal = document.querySelector(`#logs`)! as HTMLDivElement;
    if (!key)
      key = '';

    let parsedObj: any;
    try {
      parsedObj = JSON.parse(dataToDisplay);
      dataToDisplay = JSON.stringify(parsedObj, null, 4);
    } catch (error) {
      parsedObj = dataToDisplay;
    }

    if (typeof parsedObj === 'object' && parsedObj) {
      logsModal.innerHTML += `<b style='color:#574fa9;'>TS Operation:</b> (${new Date().toLocaleString()}) : ${message}: <b>${key}</b>  <span> <a href="#" class='pull-right copyMeInMemory' id="copyButton"><i class="fa fa-file-o"></i></a></span>  <pre class='displaylogs contentToCopy'>${GenericFunctionality.syntaxHighlight(dataToDisplay)}</pre> <br>`;
    } else {
      if (dataToDisplay === undefined) {
        dataToDisplay = 'Undefined object value!'
      }

      if (dataToDisplay !== '') {
        logsModal.innerHTML += `<b style='color:#574fa9;'>TS Operation:</b> (${new Date().toLocaleString()}): ${message}: <b>${key}</b> <span>  <a href="#" class='pull-right copyMeInMemory' id="copyButton"><i class="fa fa-file-o"></i></a></span> <pre class='contentToCopy'>${GenericFunctionality.syntaxHighlight(dataToDisplay)}</pre> <br>`;
      }
      else {
        logsModal.innerHTML += `<b style='color:#574fa9;'>TS Operation:</b> (${new Date().toLocaleString()}): ${message}: <b>${key}</b><br>`;
      }
    }

    let nodeToCopy = document.getElementsByClassName('copyMeInMemory')!;

    for (let node of nodeToCopy) {
      node.addEventListener('click', function (this) {
        //getting clicked element.
        let ele = (this as HTMLAnchorElement).parentElement?.nextElementSibling;
        GenericFunctionality.CopyToClipboard(ele);

      }, false);

    }
}

  /**
* Get key and value pairs from obj object.
* @param obj object on which structured key value pair object need to create.
* @param prefix prefix value.
* @returns structured KeyValuePair object array.
*/
  static getKeyValuePairs(obj: object, prefix: string = ''): _type.KeyValuePair[] {
    let pairs: _type.KeyValuePair[] = [];

    for (let key in obj) {
      if (Object.prototype.hasOwnProperty.call(obj, key)) {
        let newKey = prefix ? `${prefix}.${key}` : key;
        let value = (obj as any)[key];

        if (typeof value === 'object' && value !== null && !Array.isArray(value)) {
          pairs = pairs.concat(GenericFunctionality.getKeyValuePairs(value, newKey));
        } else {
          pairs.push({ key: newKey.toUpperCase(), value: value });
        }
      }
    }

    return pairs;
  }

  /**
   * Remove options HTMLSelectElement element..
   * @param selectElement HTMLSelectElement element.
    * @returns void.
   */
  static removeOptions(selectElement: HTMLSelectElement) {
    GenericFunctionality.HtmlLogs(`removeOptions`, `Removing existing options from department dropdown!`, '');
    while (selectElement.options.length) {
      selectElement.remove(0);
    }
    GenericFunctionality.HtmlLogs(`removeOptions`, `options removed.!`, '');
  }

  /**
 * Adding default record to Select Element
 * @param defaultContent string which is added as a default record.
 * @param departmentListElement HTMLSelectElement element on which defaultContent get added.
  * @returns void.
 */
  static addDefaultRecordOnSelectElement(defaultContent: string, departmentListElement: HTMLSelectElement) {
    GenericFunctionality.HtmlLogs(`addDefaultRecordOnSelectElement`, `Adding default rcord on !`, departmentListElement.name);
    let el = document.createElement("option");
    el.textContent = defaultContent;
    el.value = `-1`;
    departmentListElement.appendChild(el);
    GenericFunctionality.HtmlLogs(`addDefaultRecordOnSelectElement`, `Default rcord on !`, departmentListElement.name);
  }
  static toCamelCase(str): string {

    if (str.indexOf(`.`) > 0) {

      let arrOfProperty: string[] = str.split(".");
      let concateString: string = ``;
      arrOfProperty.forEach((arrElement, index) => {
        concateString = concateString + arrElement
          .replace(/\s(.)/g, function ($1) { return $1.toUpperCase(); })
          .replace(/\s/g, '')
          .replace(/^(.)/, function ($1) { return $1.toLowerCase(); });
        if (index != arrOfProperty.length - 1) {
          concateString = concateString + ".";
        }

      })
      return concateString;
    }


    return str
      .replace(/\s(.)/g, function ($1) { return $1.toUpperCase(); })
      .replace(/\s/g, '')
      .replace(/^(.)/, function ($1) { return $1.toLowerCase(); });
  }

  //to build api url based on request.
  /**
 * To build api url based on request.
 * @param baseUrs string which is added as a default record.
 * @param UseBrowserCache HTMLSelectElement element on which defaultContent get added.
 * @param params query string parameters as string array.
* @param params args as any array.
 * @returns return api url.
 */
  static buildUrl(baseUrs: string, UseBrowserCache: boolean, params?: string[], args?: any[]): string {
    const prms = params?.map((value, index) => {
      return `${value}=${args![index]}`;
    });
    if (params) {
      return `${baseUrs}?UseCache=${UseBrowserCache}&${prms!.join(`&`)}`;
    }
    else {
      return `${baseUrs}?UseCache=${UseBrowserCache}`;
    }

  }
}
export const InsertNodeDynamicly = function () {
  const IsFileAlreadyRendered = function (moduleName: string, tag: _type.Tag): boolean {
    const moduleNodes = document.querySelectorAll(tag);
    const moduleNodesArray = Array.from(moduleNodes);

    let listOfAvailableTags = moduleNodesArray.filter((value, index) => {

      if (tag == _type.Tag.css) {
        let cssTag = value as HTMLLinkElement;
        if (cssTag.href.toLowerCase().indexOf(moduleName.toLowerCase()) > -1) {
          let newLinkTag = document.createElement(tag) as HTMLLinkElement;
          newLinkTag.href = (moduleNodes[index] as HTMLLinkElement).href;
          newLinkTag.rel = "stylesheet";
          document.head.appendChild(newLinkTag);
          document.head.removeChild(newLinkTag);
          return true;
        }

      } else if (tag == _type.Tag.script) {
        let jsTag = value as HTMLScriptElement;
        if (jsTag.src.toLowerCase().indexOf(moduleName.toLowerCase()) > -1) {
          let newLinkTag = document.createElement(tag);
          newLinkTag.src = (moduleNodes[index] as HTMLScriptElement).src;
          document.body.appendChild(newLinkTag);
          document.body.removeChild(newLinkTag);
          return true;
        }
      }
      return false;
    })
    //if module node is available in DOM then return true
    if (listOfAvailableTags.length > 0) {
      return true;
    }
    return false;
  };
  const InsertNodeElement = function (url: string, elementTagType: _type.Tag, location: HTMLElement, version: string): boolean {

    if (elementTagType === _type.Tag.css) {
      const styleTag = document.createElement(elementTagType)! as HTMLStyleElement;
      styleTag.setAttribute('href', `${url}?v=${version}`);
      styleTag.setAttribute('rel', `stylesheet`);
      location.appendChild(styleTag);
    } else {
      const scriptTag = document.createElement(elementTagType)! as HTMLScriptElement;
      scriptTag.setAttribute('src', `${url}?v=${version}`);
      scriptTag.defer = true;
      scriptTag.type = "module";
      location.appendChild(scriptTag);
    }
    return true;
  };
  const loadJS = function (url: string, location: HTMLElement, partialViewName: string, version: string) {
    const isNodeAlreadyExists = IsFileAlreadyRendered(partialViewName, _type.Tag.script);
    if (!isNodeAlreadyExists) {
      InsertNodeElement(url, _type.Tag.script, location, version);
    }
  };
  const loadCss = function (url: string, location: HTMLElement, partialViewName: string, version: string) {
    const isNodeAlreadyExists = IsFileAlreadyRendered(partialViewName, _type.Tag.css);

    if (!isNodeAlreadyExists) {
      InsertNodeElement(url, _type.Tag.css, location, version);
    }
  };

  return {
    loadJS: loadJS,
    loadCss: loadCss,
    // InitDynamicFileFunction:InitDynamicFileFunction
  }
}()



// export const cipher = function(salt:string):any {
//     const textToChars = text => text.split('').map(c => c.charCodeAt(0));
//     const byteHex = n => ("0" + Number(n).toString(16)).substr(-2);
//     const applySaltToChar = code => textToChars(salt).reduce((a, b) => a ^ b, code);

//     return function (text) {
//         text.split('')
//         .map(textToChars)
//         .map(applySaltToChar)
//         .map(byteHex)
//         .join('')
//     };
// }

// export const decipher = function (salt: string): any {
//     const textToChars = text => text.split('').map(c => c.charCodeAt(0));
//     const applySaltToChar = code => textToChars(salt).reduce((a, b) => a ^ b, code);
//     return encoded => encoded.match(/.{1,2}/g)
//         .map(hex => parseInt(hex, 16))
//         .map(applySaltToChar)
//         .map(charCode => String.fromCharCode(charCode))
//         .join('');
// }
