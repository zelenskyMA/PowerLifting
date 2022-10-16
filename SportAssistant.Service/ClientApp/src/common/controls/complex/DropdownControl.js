import React, { useState } from 'react';
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem, InputGroup, InputGroupText } from "reactstrap";

export function DropdownControl({ data, onChange, placeholder, label, defaultValue = 0, noData = '' }) {
  var selectedItem = data.find(t => t.id === defaultValue);
  if (selectedItem) {
    placeholder = selectedItem.name;
  }

  const [isOpened, toggleControl] = useState(false);
  const [selectedText, changeSelection] = useState(placeholder);

  if (data?.length === 0) { return (<strong>{noData}</strong>); }

  return (
    <>
      <InputGroup>
        <InputGroupText>{label}</InputGroupText>

        <Dropdown isOpen={isOpened} toggle={() => toggleControl(!isOpened)} color="primary">
          <DropdownToggle caret>{selectedText}</DropdownToggle>
          <DropdownMenu container="body">
            {data.map((item) => {
              return (
                <DropdownItem active={item.id == defaultValue} key={'dd_' + item.id} onClick={() => setValue(item, changeSelection, toggleControl, onChange)}>
                  {item.name}
                </DropdownItem>
              );
            })}
          </DropdownMenu>
        </Dropdown>
      </InputGroup>
    </>
  );
}

function setValue(item, changeSelection, toggleControl, onChange) {
  changeSelection(item.name);
  toggleControl(false);

  onChange(item.id);
}
