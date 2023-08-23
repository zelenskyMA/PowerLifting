import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';
import { changeModalVisibility } from "../../stores/appStore/appActions";
import '../../styling/Common.css';

const mapStateToProps = store => {
  return {
    modal: store.app.modalInfo,
  }
}

const mapDispatchToProps = dispatch => {
  return {
    changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch)
  }
}

class GlobalModalDialogPanel extends Component {
  hideModal = () => {
    this.props.changeModalVisibility({ isVisible: false, headerText: "", buttons: [], body: () => { return (<p></p>) } });
  }

  buttonClick(onClickHandler) {
    onClickHandler();
    this.hideModal();
  }

  render() {
    return (
      <Modal isOpen={this.props.modal.isVisible} toggle={this.hideModal}>
        <ModalHeader toggle={this.hideModal}>{this.props.modal.headerText}</ModalHeader>
        <ModalBody>
          {this.props.modal.body()}
        </ModalBody>
        <ModalFooter>
          {this.props.modal.buttons.map(item =>
            <Button key={'modal_' + item.name}
              color={item.color || 'primary'} onClick={() => this.buttonClick(item.onClick)}>
              {item.name}
            </Button>
          )}
          <Button color="primary" outline onClick={this.hideModal}>Закрыть</Button>
        </ModalFooter>
      </Modal>
    );
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(GlobalModalDialogPanel);
