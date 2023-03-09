/2* version info 1.0.0 */
/* release 2022.03.03 */
import { SandboxPlayer } from "ZEPETO.Multiplay";
import { IModule } from "../IModule";

export default class MannequinModule extends IModule {
    private ChangedItems:Map<string,Map<string,string>> = new Map<string,Map<string,string>>();

    async OnCreate() {
        this.ChangedItems = new Map<string, Map<string, string>>();

        this.server.onMessage<CharacterItem[]>(MESSAGE.OnChangedItem, (client, message) => {
            //덮어쓰기 및 새로운부위 set
            if(this.ChangedItems.has(client.userId)){
                const changedItemMap = this.ChangedItems.get(client.userId);
                for (const characterItem of message) {
                    //드레스(22)인 경우 상의(19)와 하의(20)를 제거
                    if(characterItem.property == Cloth.DRESS){
                        if(changedItemMap?.has(Cloth.TOP))
                        {
                            changedItemMap.delete(Cloth.TOP);
                        }
                        if(changedItemMap?.has(Cloth.BOTTOM)){
                            changedItemMap.delete(Cloth.BOTTOM);
                        }
                    }
                    //상의(19) 또는 하의(20)인 경우  드레스를 제거
                    else if(characterItem.property ==Cloth.TOP || characterItem.property == Cloth.BOTTOM){
                        if(changedItemMap?.has(Cloth.DRESS))
                        {
                            changedItemMap.delete(Cloth.DRESS);
                        }
                    }

                    changedItemMap?.set(characterItem.property,characterItem.id);
                    console.log(`OnChangedItem old ${client.userId} : ${characterItem.property} // ${characterItem.id}`);
                }
            }
            //최초등록
            else {
                let changedItemMap:Map<string,string> = new Map<string, string>();
                for (const characterItem of message) {
                    changedItemMap.set(characterItem.property,characterItem.id);
                }
                this.ChangedItems.set(client.sessionId,changedItemMap);
            }
            
            const changedItem: ChangedItem = {
                sessionId : client.sessionId,
                characterItems : message
            };
            
            console.log(`OnChangedItem :  ${changedItem.sessionId}`);
            for (const characterItem of changedItem.characterItems) {
                console.log(` :::  ${characterItem.property}  - ${characterItem.id}  `);
            }
            this.server.broadcast(MESSAGE.SyncChangedItem, changedItem, { except: client });
        });

        this.server.onMessage<string>(MESSAGE.CheckChangedItem,(client, message) => {
            if(false == this.ChangedItems.has(message)){
                return;
            }
            const changedItem: ChangedItem = {
                sessionId : client.sessionId,
                characterItems : []
            };

            const values = this.ChangedItems.get(message);
            if (values !== null && values !== undefined) {
                for (const property of values.keys()) {
                    const id = values.get(property);
                    if (id === null || id === undefined) continue;
                    
                    const characterItem: CharacterItem = {
                        property,
                        id
                    };
                    changedItem.characterItems.push(characterItem);
                }
            }
            client.send<ChangedItem>(MESSAGE.SyncChangedItem, changedItem );
        });
    }

    async OnJoin(client: SandboxPlayer) {
    }

    async OnLeave(client: SandboxPlayer) {
    }

    OnTick(deltaTime: number) {
    }

}

interface CharacterItem{
    id:string;
    property:string;
}

interface ChangedItem{
    sessionId:string;
    characterItems:CharacterItem[];
}

enum Cloth{
    TOP = "19",
    BOTTOM= "20" ,
    DRESS= "22"
}

enum MESSAGE {
    OnChangedItem = "OnChangedItem",
    SyncChangedItem = "SyncChangedItem",
    CheckChangedItem = "CheckChangedItem"
}
