import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Headerdata } from './header-data.model';

@Injectable({
  providedIn: 'root'
})
export class HeaderService {

  private _headerData = new BehaviorSubject<Headerdata>({
    title: 'Início',
    icon: 'home',
    routeUrl: ''
  })
  constructor() { }

  get headerData(): Headerdata{
    return this._headerData.value
  }

  set headerData(headerData: Headerdata){
    this._headerData.next(headerData)
  }
}
