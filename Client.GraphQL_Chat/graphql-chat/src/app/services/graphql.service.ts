import { Injectable } from '@angular/core';
import { Apollo, gql } from 'apollo-angular';
import { MessageAddedGQL } from '../../generated/graphql';

@Injectable({
  providedIn: 'root'
})
export class GraphqlService {
  private name: string = '';
  constructor(
    private readonly apollo: Apollo,
    private readonly messageAddedSubscription: MessageAddedGQL
  ) { 

    this.name = this.generateRandomName();
  }

  private MESSAGE_RECEIVED_SUBSCRIPTION = gql`
    subscription {
      messageAdded {
        content
        sentAt
        from {
          id
        }
      }
    }
  `;



  sendMessage(message: string){
    let messageMutation = gql`
        mutation {
        addMessage(message: {
          fromId: "${this.name}",
          content: "${message}",
          sentAt: "${this.getCurrentIsoTimestamp()}"
        }) {
          content
          sentAt
          from {
            id
            displayName
          }
        }
      }
    `;

    this.apollo.mutate({
      mutation: messageMutation,
      variables: {}
    })
    .subscribe({
      next: (data) => {
        console.log(data, 'mutation');
      },
      error: (error) => console.log('error', error)
    })
  }

  subscribeToChat(){
    return this.messageAddedSubscription.subscribe();
    // return this.apollo.subscribe({
    //   query: this.MESSAGE_RECEIVED_SUBSCRIPTION
    // });
  }


  private generateRandomName(): string {
    const firstNames = ['Luna', 'Kai', 'Nova', 'Arlo', 'Zara', 'Leo', 'Mira', 'Ezra', 'Nia', 'Jude'];
    const lastNames = ['Rivera', 'Stone', 'Blake', 'Quinn', 'Hayes', 'Wells', 'Hart', 'Reed', 'Fox', 'Lane'];
  
    const first = firstNames[Math.floor(Math.random() * firstNames.length)];
    const last = lastNames[Math.floor(Math.random() * lastNames.length)];
  
    return `${first} ${last}`;
  }
  

  private getCurrentIsoTimestamp(): string {
    const now = new Date();
  
    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0'); // Months are 0-based
    const day = String(now.getDate()).padStart(2, '0');
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    const seconds = String(now.getSeconds()).padStart(2, '0');
  
    return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
  }
  
}
